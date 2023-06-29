create table Availability(
availability_id int PRIMARY KEY IDENTITY,
is_available bit,
date date,
start_time datetime,
end_time datetime,
user_id int,
FOREIGN KEY (user_id) REFERENCES Users(user_id)
)
