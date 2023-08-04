create table Specialization(
Specialization_Id int primary key identity,
secialization_Name varchar(255),
Hospital_Id int,
FOREIGN KEY (Hospital_Id) REFERENCES Hospital(Hospital_Id)
)