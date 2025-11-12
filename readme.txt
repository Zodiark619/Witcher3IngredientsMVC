Witcher 3 Ingredients Manager (ASP.NET MVC)

A simple web application built with ASP.NET Core MVC and Entity Framework Core that allows users to manage items, categories, and crafting recipes — inspired by The Witcher 3. Dataset gathered from https://witcher.fandom.com/wiki/The_Witcher_3_alchemy_ingredients.

Features

-CRUD Operations
Create, read, update, and delete Items, Categories, and Recipes.
-Authentication & Authorization
SuperAdmin role can add, edit, and delete data.
Guests can only view data.
-Relational Data Modeling
Items belong to categories.
Recipes require multiple items as ingredients.
-Responsive UI
Built with Razor views and Bootstrap.
-Data Validation & Security
Entity Framework Core handles database operations safely. Database used is SQLite. Identity is used to login and logout as superadmin. Datatables.js is used in index.cshtml.

How to Run Locally

-Clone the Repository
git clone https://github.com/Zodiark619/Witcher3IngredientsMVC.git
cd Witcher3IngredientsMVC
-Setup Database
dotnet ef database update
-Run the Application
dotnet run
-Access the App
Go to https://localhost:7081

Default Admin Credentials (for demo)
-username=superadmin@bloggie.com | password=Superadmin@123