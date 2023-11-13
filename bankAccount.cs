using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace L6e4
{
    class BankAccount
    {
        public string AccountNumber { get; set; } //Số tài khoản
        public string Owner { get; set; } //Chủ sở hữu
        public long Balance { get; set; } //số dư
        public string BankName { get; set; }//ngân hàng phát hành
        public string ExpiredDate { get; set; } //thời điểm hết hạn: tháng/năm
        public string PIN { get; set; }//mã pin

        //phương thức khởi tạo
        public BankAccount(string accountNumber, string owner, long balance,
            string bankName, string expiredDate, string pin)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            Balance = balance;
            BankName = bankName;
            ExpiredDate = expiredDate;
            PIN = pin;
        }
        //phương thức kiểm tra số dư
        public void CheckBalance()
        {
            Console.WriteLine($"==> Thong tin so du cua tai khoan {AccountNumber}:");
            Console.WriteLine($"So du trong tai khoan hien tai: {Balance,2:f}d");
        }

        /// <summary>
        /// phương thức nạp tiền vào tài khoản
        /// Khi số tiền được nạp vào > 0 thì cộng vào số tiền hiện có của tài khoản
        /// </summary>
        /// <param name="amount"> Số tiền nạp vào tài khoản </param>
        /// <returns>Số tiền đã nạp vào tài khoản nếu amount>0 và return 0 nếu amount <0 </returns>
        public long Deposit(long amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return amount;
            }
            return 0;
        }

        /// <summary>
        /// Phương thức rút tiền từ tài khoản X
        /// Nếu số tiền rút < số tiền trong TK và 
        /// sau khi rút còn trên 50k thì rút được
        /// </summary>
        /// <param name="amount"> Số tiền cần rút </param>
        /// <returns>Số tiền đã rút nếu thoả đk, ngược lại return 0</returns>
        public long Withdraw(long amount)
        {
            if (amount > 0 && amount < Balance - 50000)
            {
                Balance -= amount;
                return amount;
            }
            return 0;
        }
        /// <summary>
        /// Phương thức chuyển tiền đến tài khoản người khác
        /// Nếu tài khoản người đó != null và số tiền rút '<' số tiền trong tk 
        /// và sau khi rút còn '>' 50k thì cho phép
        /// Sau khi rút thì bản thân bị trừ đi amount tiền và other cộng thêm amount tiền
        /// </summary>
        /// <param name="other">Đối tượng cần chuyển tiền</param>
        /// <param name="amount">Số tiền cần chuyển</param>
        /// <returns>Số tiền đã được chuyển thành công</returns>
        public long Transfer(BankAccount other, long amount)
        {
            if (other != null && amount < Balance - 50000)
            {
                other.Balance += amount;
                this.Balance -= amount;
                return amount;
            }
            return 0;
        }
    }
    class L6e4
    {
        static void Main()
        {
            int choice;
            var accounts = new List<BankAccount>();
            do
            {
                Console.Clear();
                Console.WriteLine("================== CAC CHUC NANG ==================");
                Console.WriteLine("1. Tao moi tai khoan.");
                Console.WriteLine("2. Kiem tra so du cua tai khoan.");
                Console.WriteLine("3. Nap tien vao tai khoan.");
                Console.WriteLine("4. Rut tien khoi tai khoan.");
                Console.WriteLine("5. Chuyen tien sang tai khoan khac.");
                Console.WriteLine("6. Hien thi danh sach tai khoan ra man hinh.");
                Console.WriteLine("7. Ket thuc chuong trinh.");
                Console.Write("Xin moi ban chon chuc nang: ");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        {
                            var acc = CreateNewBankAccount();
                            accounts.Add(acc);
                            Console.WriteLine("===> DANG KY THANH CONG! <===");
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Nhap so tai khoan: ");
                            var accNumber = Console.ReadLine();
                            Console.Write("Nhap ma PIN: ");
                            var pin = Console.ReadLine();
                            var acc = CheckAccount(accNumber, pin, accounts);
                            if (acc != null)
                                ShowAccountInfo(acc);
                            else
                                Console.WriteLine("===> TAI KHOAN KHONG TON TAI <===");
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            Deposit(accounts);
                            break;
                        }
                    case 4:
                        {
                            Withdraw(accounts);
                            break;
                        }
                    case 5:
                        {
                            Transfer(accounts);
                            break;
                        }
                    case 6:
                        {
                            ShowAccounts(accounts);
                            break;
                        }
                    case 7:
                        Console.WriteLine("==> Chuong trinh ket thuc <==");
                        break;
                    default:
                        Console.WriteLine("==> Sai chuc nang. Vui long chon lai!");
                        break;
                }
            } while (choice != 7);
        }

        /// <summary>
        /// Phương thức chuyển tiền đến tài khoản khác
        /// </summary>
        /// <param name="accounts"> danh sách các tài khoản hiện có</param>
        private static void Transfer(List<BankAccount> accounts)
        {
            Console.Write("Nhap so tai khoan: ");
            var accNumber = Console.ReadLine();
            Console.Write("Nhap ma PIN: ");
            var pin = Console.ReadLine();
            var acc = CheckAccount(accNumber, pin, accounts);
            if (acc != null)
            {
                Console.Write("Nhap so tai khoan muon chuyen tien: ");
                var otherAccNumber = Console.ReadLine();
                var other = CheckAccount(otherAccNumber, accounts);
                if (other != null)
                {
                    Console.Write("Nhap so tien muon chuyen: ");
                    var amount = long.Parse(Console.ReadLine());
                    var result = acc.Transfer(other, amount);
                    if (result > 0)
                        Console.WriteLine("===> CHUYEN TIEN THANH CONG <===");
                    else
                        Console.WriteLine("===> CHUYEN TIEN THAT BAI <===");
                }
                else
                    Console.WriteLine("===> KHACH HANG KHONG TON TAI <===");
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Phương thức nạp tiền từ tài khoản
        /// </summary>
        /// <param name="accounts">Danh sách các tài khoản hiện có</param>
        private static void Deposit(List<BankAccount> accounts)
        {
            Console.Write("Nhap so tai khoan: ");
            var accNumber = Console.ReadLine();
            Console.Write("Nhap ma PIN: ");
            var pin = Console.ReadLine();
            var acc = CheckAccount(accNumber, pin, accounts);
            if (acc != null)
            {
                Console.Write("So tien can nap: ");
                var amount = long.Parse(Console.ReadLine());
                var result = acc.Deposit(amount);
                if (result > 0)
                    Console.WriteLine("===> NAP TIEN THANH CONG <===");
                else
                    Console.WriteLine("===> NAP TIEN THAT BAI <===");
            }
            else
                Console.WriteLine("===> TAI KHOAN KHONG TON TAI <===");
            Console.ReadKey();
        }

        /// <summary>
        /// Phương thức rút tiền từ tài khoản
        /// </summary>
        /// <param name="accounts">Danh sách các tài khoản hiện có</param>
        private static void Withdraw(List<BankAccount> accounts)
        {
            Console.Write("Nhap so tai khoan: ");
            var accNumber = Console.ReadLine();
            Console.Write("Nhap ma PIN: ");
            var pin = Console.ReadLine();
            var acc = CheckAccount(accNumber, pin, accounts);
            if (acc != null)
            {
                Console.Write("So tien can rut: ");
                var amount = long.Parse(Console.ReadLine());
                var result = acc.Withdraw(amount);
                if (result > 0)
                    Console.WriteLine("===> RUT TIEN THANH CONG <===");
                else
                    Console.WriteLine("===> RUT TIEN THAT BAI <===");
            }
            else
                Console.WriteLine("===> TAI KHOAN KHONG TON TAI <===");
            Console.ReadKey();
        }

        /// <summary>
        /// Phương thức kiểm tra tài khoản có tồn tại hay không với 3 đối số
        /// </summary>
        /// <param name="accNumber"></param>
        /// <param name="pin"></param>
        /// <param name="accounts"></param>
        /// <returns>tài khoản nếu tồn tại, ngược lại return null</returns>
        private static BankAccount CheckAccount(string accNumber, string pin, List<BankAccount> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                BankAccount acc = accounts[i];
                if (acc.AccountNumber == accNumber && acc.PIN == pin)
                    return acc;
            }
            return null;
        }

        /// <summary>
        /// Phương thức kiểm tra tài khoản có tồn tại hay không với 2 đối số
        /// </summary>
        /// <param name="otherAccNumber"></param>
        /// <param name="accounts"></param>
        /// <returns>Tài khoản tồn tại, ngược lại return null</returns>
        private static BankAccount CheckAccount(string otherAccNumber, List<BankAccount> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                BankAccount acc = accounts[i];
                if (acc.AccountNumber == otherAccNumber)
                    return acc;
            }
            return null;
        }
        //Hiển thị danh sách các tài khoản hiện có trong hệ thống
        private static void ShowAccounts(List<BankAccount> accounts)
        {
            Console.WriteLine("===> THONG TIN CAC TAI KHOAN <===");
            for (int i = 0; i < accounts.Count; i++)
            {
                var account = accounts[i];
                Console.WriteLine("\n==> THONG TIN TAI KHOAN THU " + i);
                ShowAccountInfo(account);
            }
            Console.ReadKey();
        }
        //hiển thị thông tin chi tiết của 1 tài khoản 
        private static void ShowAccountInfo(BankAccount account)
        {
            Console.WriteLine($"STK: {account.AccountNumber}");
            Console.WriteLine($"Owner: {account.Owner}");
            Console.WriteLine($"So du: {account.Balance,2:f}d");
            Console.WriteLine($"Ngan Hang: {account.BankName}");
            Console.WriteLine($"Thoi diem het hieu luc: {account.ExpiredDate}");
        }
        //tạo mới tài khoản, mặc định số tiền khi tạo mới là 0đ
        private static BankAccount CreateNewBankAccount()
        {
            Console.WriteLine("===> TAO MOI TAI KHOAN <===");
            Console.Write("So tai khoan: ");
            var accNumber = Console.ReadLine();
            Console.Write("Ho va ten: ");
            var owner = Console.ReadLine();
            //số dư mặc định = 0
            Console.Write("Ngan hang phat hanh: ");
            var bankName = Console.ReadLine();
            Console.Write("Thoi diem het hieu luc(VD: 12/25): ");
            var expiredDate = Console.ReadLine();
            Console.Write("Ma PIN: ");
            var pin = Console.ReadLine();
            return new BankAccount(accNumber, owner, 0, bankName, expiredDate, pin);
        }
    }

}
