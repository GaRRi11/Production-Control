# Production Control
I developed a project idea inspired by my mentor at Coca-Cola Bottlers Georgia. I made a small prototype of software that is developed for Coca-Cola bottlers Georgia and used for production registration and management. using c# and .NET I designed a Windows Forms Application. I use the MySQL library of c# to establish a connection to the database and execute SQL statements. The software deals with two entities: "Product and Modifications" which are related with ManyToOne relationship. I have implemented a modification history feature, wherein each product's alterations are recorded and linked to the respective product through a "ProductId" foreign key. the other feature of the application is its ability to export data to Excel files. Upon clicking a dedicated button, users can generate and save Excel files containing information about currently available products or the complete history of a specific product. This functionality supports efficient data analysis and reporting. 