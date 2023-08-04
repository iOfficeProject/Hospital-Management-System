create table Availability(
Availability_Id int PRIMARY KEY IDENTITY,
Is_Available bit,
Date date,
Start_Time datetime,
End_Time datetime,
User_Id int,
FOREIGN KEY (User_Id) REFERENCES Users(User_Id)
)
