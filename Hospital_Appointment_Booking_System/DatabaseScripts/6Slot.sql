create table Slot( 
Slot_Id int primary key identity,
Slot_Date datetime,
Slot_Start_Time datetime,
Slot_End_Time datetime,
Availability_Id int,
Hospital_Id int,
User_Id int,
FOREIGN KEY (Availability_Id) REFERENCES Availability(Availability_Id),
FOREIGN KEY (Hospital_Id) REFERENCES Hospital(Hospital_Id),
FOREIGN KEY (User_Id) REFERENCES Users(User_Id)
)