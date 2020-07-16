using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;

namespace CMCS.Common.Utilities
{
    public class PrintWeightReport
    {
        private static PrintWeightReport instance;

        public static PrintWeightReport GetInstance(PrintDocument printDoc)
        {
            if (instance == null)
            {
                instance = new PrintWeightReport(printDoc);
            }

            return instance;
        }

        #region Vars

        Font TitleFont = new Font("宋体", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        Font ContentFont = new Font("宋体", 14, FontStyle.Regular, GraphicsUnit.Pixel);

        PrintDocument _PrintDocument = null;

        CmcsTransportTemp _BuyFuelTransport = null;

        int PageIndex = 1;

        Graphics gs = null;

        #endregion

        public PrintWeightReport(PrintDocument printDoc)
        {
            this._PrintDocument = printDoc;
            this._PrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Custum", 690, 320);
            this._PrintDocument.OriginAtMargins = true;
            this._PrintDocument.DefaultPageSettings.Margins.Left = 0;
            this._PrintDocument.DefaultPageSettings.Margins.Right = 0;
            this._PrintDocument.DefaultPageSettings.Margins.Top = 0;
            this._PrintDocument.DefaultPageSettings.Margins.Bottom = 0;
            this._PrintDocument.PrintController = new StandardPrintController();
            this._PrintDocument.PrintPage += _PrintDocument_PrintPage;
        }

        /// <summary>
        /// 入厂煤
        /// </summary>
        /// <param name="entity"></param>
        public void PrintBuyFuelTransport(CmcsBuyFuelTransport entity)
        {
            CmcsTransportTemp temp = new CmcsTransportTemp()
            {
                TareTime = entity.TareTime,
                SupplierName = entity.SupplierName,
                CarNumber = entity.CarNumber,
                FuelKindName = entity.FuelKindName,
                GrossWeight = entity.GrossWeight,
                TareWeight = entity.TareWeight,
                SuttleWeight = entity.SuttleWeight,
                DeductWeight = entity.DeductWeight,
                Remark = entity.Remark,
                SerialNumber = entity.SerialNumber
            };

            Print(temp);
        }

        /// <summary>
        /// 其他物资
        /// </summary>
        /// <param name="entity"></param>
        public void PrintGoodsTransport(CmcsGoodsTransport entity)
        {
            CmcsTransportTemp temp = new CmcsTransportTemp()
            {
                TareTime = entity.SecondTime,
                SupplierName = entity.SupplyUnitName,
                CarNumber = entity.CarNumber,
                FuelKindName = entity.GoodsTypeName,
                GrossWeight = entity.FirstWeight,
                TareWeight = entity.SecondWeight,
                SuttleWeight = entity.SuttleWeight,
                DeductWeight = 0,
                Remark = entity.Remark,
                SerialNumber = entity.SerialNumber
            };

            Print(temp);
        }

        private void Print(CmcsTransportTemp buyfueltransport)
        {
            if (buyfueltransport == null) return;
            _BuyFuelTransport = buyfueltransport;
            try
            {
                this._PrintDocument.Print();
            }
            catch (Exception ex)
            {
                Log4Neter.Error("打印异常，请检查打印机！", ex);
            }
            _BuyFuelTransport = null;
        }

        private void _PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this.gs != null)
                g = this.gs;

            if (this._BuyFuelTransport != null)
            {
                #region 入厂煤
                // 行间距 24 
                float TopValue = 10;
                float LeftValue = 5;

                string printValue = "";
                g.DrawString("国电投青铝发电有限公司过磅单", new Font("黑体", 18, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, LeftValue, TopValue);
                TopValue += 34;

                g.DrawString("日期：" + this._BuyFuelTransport.TareTime.ToString("yyyy-MM-dd HH:mm"), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawLine(new Pen(Color.Black, 2), 0, TopValue, 300 - LeftValue, TopValue);
                TopValue += 15;

                g.DrawString("发货单位：", ContentFont, Brushes.Black, LeftValue, TopValue);
                printValue = this._BuyFuelTransport.SupplierName != null ? this._BuyFuelTransport.SupplierName : string.Empty;

                if (printValue.Length > 12)
                {
                    g.DrawString(printValue.Substring(0, 12), ContentFont, Brushes.Black, 75 + LeftValue, TopValue);
                    TopValue += 24;
                    g.DrawString(printValue.Substring(12, printValue.Length - 12), ContentFont, Brushes.Black, 75 + LeftValue, TopValue);
                    TopValue += 24;
                }
                else
                {
                    g.DrawString(printValue, ContentFont, Brushes.Black, 75 + LeftValue, TopValue);
                    TopValue += 24;
                }

                g.DrawString("车号：" + this._BuyFuelTransport.CarNumber, ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("货物名称：{0}        {1}", this._BuyFuelTransport.FuelKindName, "单位：吨"), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("毛重：{0} 吨", this._BuyFuelTransport.GrossWeight), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("皮重：{0} 吨", this._BuyFuelTransport.TareWeight), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("净量：{0} 吨", this._BuyFuelTransport.SuttleWeight), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString(string.Format("扣吨：{0} 吨", this._BuyFuelTransport.DeductWeight), ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString("备注：" + this._BuyFuelTransport.Remark, ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                g.DrawString("单号：" + this._BuyFuelTransport.SerialNumber, ContentFont, Brushes.Black, LeftValue, TopValue);
                TopValue += 24;

                TopValue += 24;

                #endregion
            }
        }

        public static string DisposeTime(string dt, string format)
        {
            if (!string.IsNullOrEmpty(dt))
            {
                DateTime dti = DateTime.Parse(dt);
                if (dti > new DateTime(2000, 1, 1))
                    return dti.ToString(format);
            }
            return string.Empty;
        }

    }

    public class CmcsTransportTemp
    {
        /// <summary>
        /// 皮重时间
        /// </summary>
        public DateTime TareTime { get; set; }
        /// <summary>
        /// 供煤单位
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 车号
        /// </summary>
        public string CarNumber { get; set; }
        /// <summary>
        /// 煤种
        /// </summary>
        public string FuelKindName { get; set; }
        /// <summary>
        /// 毛重(吨)
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// 皮重(吨)
        /// </summary>
        public decimal TareWeight { get; set; }
        /// <summary>
        /// 净重(吨)
        /// </summary>
        public decimal SuttleWeight { get; set; }
        /// <summary>
        /// 扣吨(吨)
        /// </summary>
        public decimal DeductWeight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
