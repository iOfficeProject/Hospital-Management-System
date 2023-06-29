create table Users (
user_id int PRIMARY KEY IDENTITY,
name varchar(255),
email varchar(255),
password varchar(255),
mobile_number bigint,
role_id int,
specialization_id int,
hospital_id int,
  FOREIGN KEY (role_id) REFERENCES Roles(role_id),
   FOREIGN KEY (hospital_id) REFERENCES Hospital(hospital_id),
  FOREIGN KEY (specialization_id) REFERENCES Specialization(specialization_id)
)
