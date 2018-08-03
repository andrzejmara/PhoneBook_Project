using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using PhoneBook.Helpers;

namespace PhoneBook.Models
{
	public class SourceManager
	{
		public static int Add(PersonModel personModel)
		{
			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = @"Insert INTO People (FirstName, LastName, Phone, Email, Created, Updated)
				VALUES (@FirstName,@LastName,@Phone, @Email, @Created, @Updated); SELECT CAST(scope_identity() AS int)";

				var sqlFirstNameParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.FirstName,
					ParameterName = "@FirstName"
				};
				var sqlLastNameParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.LastName,
					ParameterName = "@LastName"
				};
				var sqlPhoneParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.Phone,
					ParameterName = "@Phone"
				};
				var sqlEmailParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.Email,
					ParameterName = "@Email"
				};
				var sqlCreatedDateParam = new SqlParameter
				{
					DbType = System.Data.DbType.DateTime,
					Value = DateTime.Now,
					ParameterName = "@Created"
				};
				var sqlUpdatedDateParam = new SqlParameter
				{
					DbType = System.Data.DbType.DateTime,
					Value = personModel.Updated,
					ParameterName = "@Updated"
				};
				sqlCommand.Parameters.Add(sqlFirstNameParam);
				sqlCommand.Parameters.Add(sqlLastNameParam);
				sqlCommand.Parameters.Add(sqlPhoneParam);
				sqlCommand.Parameters.Add(sqlEmailParam);
				sqlCommand.Parameters.Add(sqlCreatedDateParam);
				sqlCommand.Parameters.Add(sqlUpdatedDateParam);

				return (int)sqlCommand.ExecuteScalar();
			}
		}

		public static void Update(PersonModel personModel)
		{
			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = @"UPDATE People 
				SET FirstName = @FirstName, LastName = @LastName,Phone = @Phone, Email = @Email,Created = @Created, Updated = @Updated
				WHERE ID = @ID";

				var sqlFirstNameParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.FirstName,
					ParameterName = "@FirstName"
				};

				var sqlLastNameParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.LastName,
					ParameterName = "@LastName"
				};

				var sqlPhoneParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.Phone,
					ParameterName = "@Phone"
				};
				var sqlEmailParam = new SqlParameter
				{
					DbType = System.Data.DbType.AnsiString,
					Value = personModel.Email,
					ParameterName = "@Email"
				};
				var sqlCreatedDateParam = new SqlParameter
				{
					DbType = System.Data.DbType.DateTime,
					Value = personModel.Created,
					ParameterName = "@Created"
				};

				var sqlUpdatedDateParam = new SqlParameter
				{
					DbType = System.Data.DbType.DateTime,
					Value = DateTime.Now,
					ParameterName = "@Updated"
				};
				var sqlIDParam = new SqlParameter
				{
					DbType = System.Data.DbType.Int32,
					Value = personModel.ID,
					ParameterName = "@ID"
				};

				sqlCommand.Parameters.Add(sqlFirstNameParam);
				sqlCommand.Parameters.Add(sqlLastNameParam);
				sqlCommand.Parameters.Add(sqlPhoneParam);
				sqlCommand.Parameters.Add(sqlEmailParam);
				sqlCommand.Parameters.Add(sqlCreatedDateParam);
				sqlCommand.Parameters.Add(sqlUpdatedDateParam);
				sqlCommand.Parameters.Add(sqlIDParam);
				sqlCommand.ExecuteScalar();
			}
			return;
		}

		public static void Remove(int id)
		{
			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = "DELETE FROM People WHERE ID = @ID; ";

				var sqlIDParam = new SqlParameter
				{
					DbType = System.Data.DbType.Int32,
					Value = id,
					ParameterName = "@ID"
				};
				sqlCommand.Parameters.Add(sqlIDParam);
				sqlCommand.ExecuteNonQuery();
			}
		}

		public static PersonModel GetById(int id)
		{
			PersonModel personModel;
			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = "SELECT * FROM People WHERE ID = @ID;";

				var sqlIDParam = new SqlParameter
				{
					DbType = System.Data.DbType.Int32,
					Value = id,
					ParameterName = "@ID"
				};
				sqlCommand.Parameters.Add(sqlIDParam);
				var data = sqlCommand.ExecuteReader();
				data.Read();

				personModel = new PersonModel(id, data["FirstName"].ToString(), data["LastName"].ToString(),
					data["Phone"].ToString(),
					data["Email"].ToString(), DateTime.Parse(data["Created"].ToString()), DateTime.Parse(data["Updated"].ToString()));
			}
			return personModel;
		}

		public static List<PersonModel> Get(int start, int take)
		{
			var personList = new List<PersonModel>();

			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = "SELECT * FROM People ORDER BY ID OFFSET @Start ROWS FETCH NEXT @Take ROWS ONLY;";

				var sqlStartParam = new SqlParameter
				{
					DbType = System.Data.DbType.Int32,
					Value = (start - 1) * take,
					ParameterName = "@Start"
				};

				var sqlTakeParam = new SqlParameter
				{
					DbType = System.Data.DbType.Int32,
					Value = take,
					ParameterName = "@Take"
				};
				sqlCommand.Parameters.Add(sqlStartParam);
				sqlCommand.Parameters.Add(sqlTakeParam);

				var data = sqlCommand.ExecuteReader();

				while (data.HasRows && data.Read())
				{
					personList.Add(new PersonModel((int)data["ID"],
					data["FirstName"].ToString(),
					data["LastName"].ToString(),
					data["Phone"].ToString(),
					data["Email"].ToString(),
					(DateTime)data["Created"],
					(DateTime?)data["Updated"]
					));
				}
			}

			return personList;
		}

		public static List<PersonModel> Search(string what, string where)
		{
			var searchList = new List<PersonModel>();

			using (var connection = SqlHelper.GetConnection())
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.CommandText = $"SELECT * FROM People WHERE {where} LIKE @What;";

				sqlCommand.Parameters.AddWithValue("@What", what);
				var data = sqlCommand.ExecuteReader();

				while (data.HasRows && data.Read())
				{
					searchList.Add(new PersonModel((int)data["ID"],
						data["FirstName"].ToString(),
						data["LastName"].ToString(),
						data["Phone"].ToString(),
						data["Email"].ToString(),
						(DateTime)data["Created"],
						(DateTime?)data["Updated"]
					));
				}
			}
			return searchList;
		}
	}
}

