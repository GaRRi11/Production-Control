### Production Control
## Project Commissioned by Coca-Cola Bottlers Georgia Lead Developer
The “Production Control” Project idea was to think from the users' point of view and therefore plan the suitable UI and application functionality. I was responsible for making all of the decisions for the business logic and architecture of the app by myself. I received positive feedback, Also some functionality logic became an inspiration for refactoring the main company app of accounting. 

Users can easily register production groups by specifying names, packaging types, and liters, as well as cities by providing names and warehouse capacities. And they can register actual products by specifying product groups, names, prices, expiration dates, and cities. The application enables users to modify all registered information effortlessly.

In the product Modify Form, users have comprehensive control over product information. They can modify product details, transfer products between cities, adjust product quantities, delete products, and view product history. Notably, all operations, such as transfers between cities and quantity adjustments, are recorded and stored in the database. Users can access the modification history of a specific product by simply clicking on the record icon in the product modification form.

In the project, a Service Layer architecture is implemented, where each entity is managed by its service class. This ensures that accessing and modifying entity information is only possible through its respective service class. Four main entities are utilized: Product, ProductGroup, City, and Modifications.

before the user registers the product, he needs to register the city and product group. when the city is being registered, in form tab page with the city name and capacity is added and when a user registers the product city is chosen by the selected tab page. Additionally, users specify the product group, which serves as a category for redistributing products. A combo box positioned in the upper-left corner of the main form facilitates the selection of the product group. Upon choosing a product group, the application dynamically displays the relevant products. Another combo box allows users to filter products based on expiration dates, such as showing only products with one month remaining until expiration.

The application adheres to the principles of atomicity during database communications, ensuring transactional integrity. Various methods, like the transfer method for changing a product's city, involve multiple SQL queries. If any part of these transactions fails, the system rolls back all changes to maintain consistency.
This approach enhances data management and user experience, promoting modularity, reliability, and intuitive interaction with the application's functionalities.

![Forms](https://github.com/GaRRi11/Production-Control/assets/101354276/7ed32c98-80c3-4112-a45e-ead43f6e7242)
