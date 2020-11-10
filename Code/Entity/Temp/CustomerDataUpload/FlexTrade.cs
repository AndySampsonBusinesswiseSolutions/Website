using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class FlexTrade
            {
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
                }

                private int _RowId;
                public int RowId
                {
                    get { return _RowId; }
                    set { _RowId = value; }
                }

                private string _BasketReference;
                public string BasketReference
                {
                    get { return _BasketReference; }
                    set { _BasketReference = value; }
                }

                private string _TradeReference;
                public string TradeReference
                {
                    get { return _TradeReference; }
                    set { _TradeReference = value; }
                }

                private string _TradeDate;
                public string TradeDate
                {
                    get { return _TradeDate; }
                    set { _TradeDate = value; }
                }

                private string _TradeProduct;
                public string TradeProduct
                {
                    get { return _TradeProduct; }
                    set { _TradeProduct = value; }
                }

                private string _Volume;
                public string Volume
                {
                    get { return _Volume; }
                    set { _Volume = value; }
                }

                private string _Price;
                public string Price
                {
                    get { return _Price; }
                    set { _Price = value; }
                }

                private string _Direction;
                public string Direction
                {
                    get { return _Direction; }
                    set { _Direction = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public FlexTrade(Guid ProcessQueueGUID_,int RowId_,string BasketReference_,string TradeReference_,string TradeDate_,string TradeProduct_,string Volume_,string Price_,string Direction_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.BasketReference = BasketReference_;
                    this.TradeReference = TradeReference_;
                    this.TradeDate = TradeDate_;
                    this.TradeProduct = TradeProduct_;
                    this.Volume = Volume_;
                    this.Price = Price_;
                    this.Direction = Direction_;
                    this.CanCommit = CanCommit_;
                }

                public FlexTrade(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.BasketReference = (string)dataRow["BasketReference"];
                    this.TradeReference = (string)dataRow["TradeReference"];
                    this.TradeDate = (string)dataRow["TradeDate"];
                    this.TradeProduct = (string)dataRow["TradeProduct"];
                    this.Volume = (string)dataRow["Volume"];
                    this.Price = (string)dataRow["Price"];
                    this.Direction = (string)dataRow["Direction"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}