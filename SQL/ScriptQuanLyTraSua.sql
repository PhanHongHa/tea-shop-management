INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'Admin', 
	N'Admin',
	N'1',
	1
)

INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'Employee',
	N'Employee',
	N'1',
	0
)

INSERT INTO dbo.Account (
	UserName ,
	DisplayName ,
	Password ,
	[Type] 

)
VALUES(
	N'IT',
	N'PhanHongHa',
	N'1',
	1
)


SELECT * FROM Account

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END

DROP  PROC USP_GetAccountByUserName

EXEC dbo.USP_GetAccountByUserName @userName = N'IT'


CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND Password = @passWord
END

USP_Login @userName = N'IT' , @passWord = N'1'

--thêm Table
DECLARE @i INT = 1
WHILE @i <= 20
BEGIN
	INSERT dbo.TableFood(name) 
	VALUES (N'Bàn ' + CAST(@i As nvarchar(100)))
	SET @i = @i +1
END

SELECT * FROM TableFood 


CREATE PROC USP_GetTableList
AS
SELECT * FROM dbo.TableFood 

EXEC USP_GetTableList


--Thêm Category
INSERT dbo.FoodCategory 
		(name)
VALUES (N'Món nước')
INSERT dbo.FoodCategory 
		(name)
VALUES (N'Món ăn vặt')
INSERT dbo.FoodCategory 
		(name)
VALUES (N'Món phụ')

--Thêm Food
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa thái xanh', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa thái xanh', 1, N'L', 25000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa thái đỏ', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa thái đỏ', 1, N'L', 25000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa truyền thống', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà sửa truyền thống', 1, N'L', 25000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Sửa tươi chân trân đường đen', 1, N'M', 25000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Sửa tươi chân trân đường đen', 1, N'L', 25000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà đào', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà vải', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, size, price)
VALUES (N'Trà ổi', 1, N'M', 20000)
INSERT dbo.Food
		(name, idCategory, price)
VALUES (N'Bánh trán trộn', 2, 15000)
INSERT dbo.Food
		(name, idCategory, price)
VALUES (N'Bánh trán bùi nhùi', 2, 15000)
INSERT dbo.Food 
		(name, idCategory, price)
VALUES (N'Bánh trán xì ke', 2, 10000)
INSERT dbo.Food
		(name, idCategory, price)
VALUES (N'Trân châu đen', 3, 5000)
INSERT dbo.Food
		(name, idCategory, price)
VALUES (N'Trân châu trắng', 3, 5000)
INSERT dbo.Food
		(name, idCategory, price)
VALUES (N'Bánh plan', 3, 5000)


--Thêm Bill
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), NULL, 10, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), NULL, 11, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), NULL, 8, 0)
INSERT dbo.Bill
		(DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), GETDATE(), 9, 1)


--Thêm Bill Info
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (5, 1, 2)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (5, 2, 2)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (6, 2, 1)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (6, 5, 2)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (7, 8, 2)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (7, 6, 2)
INSERT dbo.BillInfo
		(idBill, idFood, count)
VALUES (8, 4, 2)




SELECT * FROM dbo.BillInfo WHERE idBill = 5

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo.Bill
			(DateCheckIn, DateCheckOut, idTable, status, discount)
	VALUES (GETDATE(), NULL, @idTable, 0, 0)
END


CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	INSERT dbo.BillInfo
			(idBill, idFood, count)
	VALUES (@idBill, @idFood, @count)
END


ALTER  PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	
	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1
	
	SELECT @isExitsBillInfo =id, @foodCount = count
	FROM dbo.BillInfo 
	WHERE IdBill = @idBill AND IdFood = @idFood
	
	IF (@isExitsBillInfo >0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF(@newCount > 0)
			UPDATE dbo.BillInfo  SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE 
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE 
		BEGIN 
			INSERT dbo.BillInfo
					(idBill, idFood, count)
			VALUES (@idBill, @idFood, @count)
		END
	
END



ALTER  TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo  FOR INSERT , UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM Inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status =0
	
	DECLARE @count INT
	SELECT  @count = COUNT(*) FROM dbo.BillInfo WHERE  idBill = @idBill
	
	IF (@count > 0)
		BEGIN
			UPDATE dbo.TableFood  SET status = N'Có Người' Where id = @idTable
		END
	ELSE 
		BEGIN
			UPDATE dbo.TableFood  SET status = N'Trống' Where id = @idTable
		END
		
		
	
END

CREATE  TRIGGER UTG_UpdateTable
ON dbo.TableFood  FOR UPDATE 
AS
BEGIN
	DECLARE @idTable INT
	
	DECLARE @status NVARCHAR(100)
	SELECT  @idTable = id, @status = Inserted.status FROM Inserted
	
	DECLARE @idBill INT
	SELECT @idBill = id FROM dbo.Bill WHERE idTable = @idTable AND status = 0
	
	DECLARE @countBillInfo INT
	SELECT @countBillInfo = COUNT(*) FROM dbo.BillInfo WHERE idBIll = @idBill
	
	IF(@countBillInfo > 0 )--AND @status <> N'Có người')
		UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
	ELSE-- IF (@countBillInfo <= 0 AND status = N'Trống')
		UPDATE  dbo.TableFood SET status = N'Trống'  WHERE id = @idTable
END



CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBILL = id FROM Inserted
	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE  idTable = @idTable AND status =0
	IF(@count = 0)
		UPDATE dbo.TableFood SET  status = N'Trống' WHERE id = @idTable
END

ALTER  TABLE dbo.Bill 
ADD discount INT

UPDATE dbo.Bill SET discount = 0

CREATE  PROC USP_SwitchTable
@idTable1 INT, @idTable2 INT
AS BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSeconrdBill INT
	
	DECLARE @isFirstTableEmty INT = 1
	DECLARE @isSeconrdTableEmty INT = 1
	
	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idTable  = @idTable2 AND status =0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable  = @idTable1 AND status =0

	IF (@idFirstBill is NULL)
	BEGIN
		INSERT dbo.Bill
				(DateCheckIn, DateCheckOut, idTable, status, discount)
		VALUES (GETDATE(), NULL, @idTable1, 0, 0)
		
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE IdTable  = @idTable1 AND Status = 0
	
		
		
	END
	
	SELECT @isFirstTableEmty = COUNT(*) FROM dbo.BillInfo WHERE IdBill = @idFirstBill 
	
	IF (@idSeconrdBill is NULL)
	BEGIN
		INSERT dbo.Bill
				(DateCheckIn, DateCheckOut, idTable, status, discount)
		VALUES (GETDATE(), NULL, @idTable2, 0, 0)
		
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE IdTable  = @idTable2 AND Status = 0
	
		
		
	END
	SELECT @isSeconrdTableEmty = COUNT(*) FROM dbo.BillInfo WHERE IdBill = @idSeconrdBill 
	
	
	SELECT  id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill =@idSeconrdBill
	
	UPDATE  dbo.BillInfo  SET IdBill  = @idSeconrdBill WHERE  IdBill  = @idFirstBill
	
	UPDATE  dbo.BillInfo SET  IdBill  = @idFirstBill WHERE  id IN (SELECT *FROM IDBillInfoTable)
	
	DROP TABLE IDBillInfoTable
	
	IF(@isFirstTableEmty = 0)
		UPDATE  dbo.TableFood SET status = N'Trống' WHERE id = @idTable2
		
	IF(@isSeconrdTableEmty = 0)
		UPDATE  dbo.TableFood SET status = N'Trống' WHERE id = @idTable1
 
END



CREATE PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS
BEGIN
	SELECT t.name [Tên bàn], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá], b.totalPrice AS [Tổng tiền]
	FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status =1
	AND t.id = b.idTable
END



CREATE  PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	
	SELECT  @isRightPass = COUNT(*) FROM dbo.Account  WHERE  userName = @userName AND password = @password
	
	IF(@isRightPass =1)
	BEGIN
		IF (@newPassword = NULL OR @newPassword = '')
			BEGIN 
				UPDATE dbo.Account SET DisplayName = @displayName WHERE  userName = @userName
			END
		ELSE 
			UPDATE dbo.Account SET displayName = @displayName, password = @newPassword WHERE  userName = @userName
	END
	
END



ALTER  PROCEDURE UpdateUser (
    @userName VARCHAR(50),
    @displayName VARCHAR(100),
    @currentPassword VARCHAR(255), -- Mật khẩu hiện tại
    @newPassword VARCHAR(255) -- Mật khẩu mới
   
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra mật khẩu hiện tại
    DECLARE @storedPassword VARCHAR(255);
    SELECT @storedPassword = password
    FROM Account 
    WHERE userName = @userName;

    IF @currentPassword <> @storedPassword
    BEGIN
        RAISERROR ('Mật khẩu hiện tại không chính xác.', 16, 1);
        RETURN;
    END;

    -- Cập nhật thông tin người dùng
   	IF (@newPassword = NULL OR @newPassword = '')
   	BEGIN 
   		UPDATE dbo.Account 
    	SET displayName = @displayName
    	WHERE userName = @userName;
   	END
   	
   

    -- Nếu mật khẩu mới được cung cấp, cập nhật mật khẩu mới
    ELSE
    BEGIN
        UPDATE dbo.Account 
        SET password = @newPassword
        WHERE userName = @userName;
    END;
END;


CREATE TRIGGER UTG_DeleteBillInfo
On dbo.BillInfo  FOR DELETE 
AS
BEGIN 
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM Deleted
	
	DECLARE @idTable INT 
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count INT = 0 
	SELECT @count = COUNT(*) FROM dbo.BillInfo AS bi, dbo.Bill AS b WHERE b.id = bi.idBill AND b.id = @idBill  AND b.status = 0
	
	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE  id = @idTable
	
END

-- hàm SQL xử lý tìm kiểm gần đúng chuẩn nhất

CREATE FUNCTION [dbo].[fuConvertToUnsign1] 
(@strInput NVARCHAR(4000) ) 
RETURNS NVARCHAR(4000) 
AS 
BEGIN 
	IF @strInput IS NULL 
	RETURN @strInput 
	IF @strInput = '' 
	RETURN @strInput 
	 
	DECLARE @RT NVARCHAR(4000) 
	DECLARE @SIGN_CHARS NCHAR(137) 
	DECLARE @UNSIGN_CHARS NCHAR(137) 
	SET @SIGN_CHARS = N'ăâđêơưàáảãạằắẳẵặấẩẫầậèéẻẽẹếềểễệ ìíịỉĩòóỏõọồổốỗộớờợỡởùụúủũừứửữựỳýỹỷỵ  ĂÂĐÊƠƯÀÁẢÃẠẰẮẲẴẶẤẨẪẦẬÈÉẺẼẸẾỀỂỄỆ ÌÍỊỈĨÒÓỎÕỌỒỔỐỖỘỚỜỢỠỞÙỤÚỦŨỪỨỬỮỰỲÝỸỶỴ' +NCHAR(272) + NCHAR(208) 
	SET @UNSIGN_CHARS = N'aadeouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOUAAAAAAAAAAAAAAAEEEEEEEEEE IIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYf' 
	
	DECLARE @COUNTER INT 
	DECLARE @COUNTER1 INT 
	SET @COUNTER = 1 
	WHILE (@COUNTER <= LEN(@strInput) ) 
	BEGIN  
		SET @COUNTER1 = 1 
		WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) ) 
		BEGIN 
			IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput, @COUNTER, 1) )
			BEGIN 
				IF @COUNTER=1 
					SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER+1, LEN(@strInput) - 1 ) 
				ELSE 
					SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) + SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER +1 , LEN(@strInput) - @COUNTER)
		
				BREAK 
			END 
			SET @COUNTER1 = @COUNTER1 +1 
		END 
		SET @COUNTER = @COUNTER + 1 
	END 
	SET @strInput = REPLACE(@strInput,' ','-') 
	RETURN @strInput 
END 

CREATE PROC USP_GetListFood
AS
BEGIN
	SELECT f.id [Id], f.name [Tên món], f.idCategory [Loại], f.price [Tổng tiền]
	FROM dbo.Food f 
	
END


SELECT * FROM  dbo.Food  WHERE dbo.fuConvertToUnsign1(name) LIKE N'%tra%'

EXEC UpdateUser @userName = N'IT' , @displayName = N'Phan Hồng ' , @currentPassword = '1', @newPassword = ''

EXEC USP_UpdateAccount @userName = N'IT' , @displayName = N'Phan Hồng Hà' , @password = '', @newPassword = ''

SELECT * FROM dbo.Account  WHERE  userName = 'IT' AND password = '1'

ALTER TABLE Bill ADD totalPrice FLOAT


SELECT * FROM Account 
SELECT * FROM TableFood 
SELECT * FROM BillInfo 
SELECT * FROM Bill
SELECT Id, Name, Price FROM Food 
SELECT * FROM FoodCategory 


DELETE BillInfo
DELETE Bill
