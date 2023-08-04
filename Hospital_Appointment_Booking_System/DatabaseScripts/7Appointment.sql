create table Appointment(
Appointment_Id int primary key identity, 
Appointment_Date datetime,
Appointment_Start_Time datetime,
Appointment_End_Time datetime, 
Slot_Id int,
Hospital_Id int,
User_Id int,
FOREIGN KEY (Hospital_Id) REFERENCES Hospital(Hospital_Id),
FOREIGN KEY (Slot_Id) REFERENCES Slot(Slot_Id),
FOREIGN KEY (User_Id) REFERENCES Users(User_Id)
)