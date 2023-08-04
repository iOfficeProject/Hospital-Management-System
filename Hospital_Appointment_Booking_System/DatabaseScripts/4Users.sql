create table Users (
User_Id int PRIMARY KEY IDENTITY,
Name varchar(255),
Email varchar(255),
Password varchar(255),
Mobile_Number bigint,
Role_Id int,
Specialization_Id int,
Hospital_Id int,
  FOREIGN KEY (Role_Id) REFERENCES Roles(Role_Id),
   FOREIGN KEY (Hospital_Id) REFERENCES Hospital(Hospital_Id),
  FOREIGN KEY (Specialization_Id) REFERENCES Specialization(Specialization_Id)
)
