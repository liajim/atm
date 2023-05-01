
### Installing

The project uses .net 7.

Follow these steps to get your development environment set up:
1. Clone the repository
2. Add the source for nuget packages:
```csharp
dotnet nuget add source --name nuget.org https://api.nuget.org/v3/index.json
```
3. At the root directory, restore required packages by running:
```csharp
dotnet restore
```
4. Next, build the solution by running:
```csharp
dotnet build
```
5. Next, create atm database and configure the connection string in appsettings.json.
6. Next, update database.
```csharp
dotnet ef database update
```
7. Next, launch the back end by running:
```csharp
dotnet run
```
8. Launch http://localhost:5142/swagger/index.html in your browser to view the Web UI.

### Usage
There are 3 groups of endpoints: Accounts, Clients and Transactions. Accounts and Clients have CRUD endpoints for making
the basic operations. The get single account endpoint will retrieve a field called balance witht the balance of the account.

Transactions has three endpoints for the main operations: Deposit, Withdraw and Transfer. These 3 operations are store in one table
with different transaction types. Deposit creates 1 row with positive amount, Withdraw also creates 1 row with negative amount and
Transfer creates 2 rows one with negative value in the origin account and the other with positive amount in the destination account.

The update transaction endpoint receive always positive values for amount, if transaction is deposit the amount will be updated as 
positive, if it is withdraw, amount will be updated as negative, if transaction is transfer, it will update the 2 rows related one
with positive amount and the other with negative amount.

The delete transaction endpoint cannot remove the negative amount row related to transfer alone. For removing it, endpoint will accept 
the id of the main transfer row for removing both rows.

