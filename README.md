1.	Enter your file path in the config file “config.json” in “filePath” key, Default value “./ProjectData.txt”.

2.	 File is a comma-separated, Sample file attached in the project.

3.	EmpID, ProjectID should be an integer and not null.

4.	DateFrom should be a datetime and not null.

5.	DateTo should be datetime, if empty or null will take today date as its value.

6.	 If one record has wrong data, the program will ignore it and continue with the correct records.

7.	Output will be like:
Employee ID #1: value, Employee ID #2: value, Project ID: value, Days worked: value
If the project doesn’t have pair of employees worked as a team a description msg will appear.
