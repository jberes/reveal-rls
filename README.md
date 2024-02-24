# reveal-rls


# Stored Procedures 
```
OrdersByCustomer
CREATE PROCEDURE [dbo].[OrdersByCustomer] @CustomerID nchar(5)
AS
SELECT *
FROM OrdersQry
WHERE CustomerID = @CustomerID
ORDER BY OrderID
```

OrderDetails
```
CREATE PROCEDURE [dbo].[OrderDetails] @OrderID int
AS
SELECT OrderID, ProductName,
    UnitPrice=ROUND(Od.UnitPrice, 2),
    Quantity,
    Discount=CONVERT(int, Discount * 100), 
    ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2)
FROM Products P, [Order Details] Od
WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID
```

OrdersByEmployee
```
CREATE PROCEDURE [dbo].[OrdersByEmployee]
(
    @EmployeeID INT
)
AS
BEGIN
    SELECT * from OrderAnalysis WHERE EmployeeID = @EmployeeID
END
```
