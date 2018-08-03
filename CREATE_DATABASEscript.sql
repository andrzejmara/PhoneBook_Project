CREATE DATABASE PhoneBook;
CREATE TABLE People(
ID Int Identity(1,1) NOT NULL PRIMARY KEY ,
FirstName nvarchar(32) NOT NULL,
LastName nvarchar(64) NOT NULL,
Phone nvarchar(25) NOT NULL,
Email nvarchar(256) NOT NULL,
Created DateTime,
Updated DateTime
); 


