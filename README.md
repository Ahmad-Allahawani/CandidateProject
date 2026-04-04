

##  Live Website

You can access the live website using the following link:
 [https://allahawaniahmad-001-site1.ktempurl.com/](https://allahawaniahmad-001-site1.ktempurl.com/)

---
## Table of Contents

- [Local Setup](#local-setup)
- [Database Migration](#database-migration)

---

##  Local Setup

Follow these steps to run the project locally on your machine:

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd <your-project-folder>
```

### 2. Check the .NET Version

Make sure you have the correct .NET version installed that matches the project requirements. You can verify your installed version by running:
```bash
dotnet --version
```

> Ensure the version matches the one specified in the project's `.csproj` file.

### 3. Check the Connection String

Open the `appsettings.json` file and verify that the connection string is correctly configured to point to your local database:
```json
"ConnectionStrings": {
  "DefaultConnection": "your-connection-string-here"
}
```

> Make sure the server name, database name, and credentials are correct for your local environment.

---

##  Database Migration

Follow these steps to set up the database:

### 1. Switch to the Script Branch

### 2. Locate and Download the Script

Find the SQL script file in the branch and download/copy it to your local machine.

### 3. Open the Script in SQL Server

Drag and drop the `.sql` script file directly into **SQL Server Management Studio (SSMS)**.

### 4. Execute the Script

Once the script is loaded in SSMS, click **Execute** (or press `F5`) to run the script and set up the database.

>  After successful execution, your database will be ready to use with the project.

---

