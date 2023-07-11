namespace AnalysisDashboard.Models
{
    public class DataHeader
    {
        public string Date { get; set; } = "";
        public string Mfo { get; set; } = "";
        public string BranchOfTheBank { get; set; } = "";
        public string NameOfFirm { get; set; } = "";
        public string Account { get; set; } = "";
        public string Tin { get; set; } = "";
        public double BalanceAtTheBeginningOfPeriod { get; set; } = 0.0;
        public double FxpenseForThePeriod { get; set; } = 0.0;
        public double FceiptForThePeriod { get; set; } = 0.0;
        public double BalanceAtTheEndOfThePeriod { get; set; } = 0.0;
        public string InformationAboutTheOperationOfTheAccount { get; set; } = "";

        public DataHeader()
        {
            
        }

        public void Copy(DataHeader other)
        {
            Date = other.Date;
            Mfo = other.Mfo;
            BranchOfTheBank = other.BranchOfTheBank;    
            NameOfFirm = other.NameOfFirm;
            Account = other.Account;
            Tin = other.Tin;
            BalanceAtTheBeginningOfPeriod = other.BalanceAtTheBeginningOfPeriod;
            BalanceAtTheEndOfThePeriod = other.BalanceAtTheEndOfThePeriod;
            InformationAboutTheOperationOfTheAccount = other.InformationAboutTheOperationOfTheAccount;
        }
    }

    public class DataItem
    {
        public DateTime Date { get; set; }
        public string Account_Tin { get; set; } = "";
        public string NoDokta { get; set; } = "";
        public string Op { get; set; } = "";
        public string Mfo { get; set; } = "";
        public double Debit { get; set; } = 0.0;
        public double Credit { get; set; } = 0.0;
        public string PurposeOfPayment { get; set; } = "";
        public string Code { get; set; } = "";
        public string NameOfCode { get; set; } = "";

        public void Copy(DataItem other)
        {
            Date = other.Date;
            Account_Tin= other.Account_Tin;
            NoDokta = other.NoDokta;
            Op = other.Op;
            Mfo = other.Mfo;
            Debit = other.Debit;
            Credit = other.Credit;
            PurposeOfPayment = other.PurposeOfPayment; 
            Code = other.Code;
            NameOfCode = other.NameOfCode;
        }
    }

    public class DataInfo
    {
        public DataHeader? Header { get; set; } = null;
        public List<DataItem>? Data { get; set; } = new List<DataItem>();
    }
}
