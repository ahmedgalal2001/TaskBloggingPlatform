# TaskBloggingPlatform


## Prerequisites

- .NET SDK
- SQL Server
- Entity Framework Core Tools

## Getting Started

1. **Clone the repository**:
    ```sh
    git clone https://github.com/ahmedgalal2001/TaskBloggingPlatform.git
    cd TaskBloggingPlatform
    ```

2. **Restore the database**:
    - Open SQL Server Management Studio (SSMS).
    - Connect to your local SQL Server instance.
    - Right-click on `Databases` > `Restore Database...`.
    - Select `Device` and click on `github` is called `BlogPlatform.bak` to browse and add the `.bak` file you created earlier.
    - Ensure the `Destination` database name is the same as in your application configuration.
    - Click `OK` to restore.

3. **Update connection string**:
    - Open `appsettings.json` in your project.
    - Update the connection string to point to your local SQL Server instance. For example:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=YourDatabaseName;Trusted_Connection=True;"
    }
    ```

4. **Apply migrations** (if necessary):
    ```sh
    dotnet ef database update --project full_of_project
    ```

5. **Install dependencies**:
    ```sh
    dotnet restore
    ```

6. **Run the application**:
    ```sh
    dotnet run --project full_of_project
    go to your_localhost/swagger/index.html
    ```

7. **Access the application**:
    - Open your web browser and navigate to `http://localhost:5000` or `http://localhost:5001` (depending on whether your project is configured to use HTTP or HTTPS).

## Contact

For any questions, please contact [Ahmed Galal] at [ahmedgalalmohamed912@gmail.com].
