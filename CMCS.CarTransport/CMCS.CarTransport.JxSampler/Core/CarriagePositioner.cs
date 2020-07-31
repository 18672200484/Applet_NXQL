using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace CMCS.CarTransport.JxSampler.Core
{
	/**
    *   汽车机械采样与火车机械采样车厢定位生成类
    **/

	/// <summary>
	/// 汽车车厢采样定位类
	/// 以副驾驶侧的车厢尾端为坐标原点
	/// </summary>
	public class TruckCarriagePositioner
	{
		static Random StaticRandom = new Random();

		public TruckCarriagePositioner(TruckMeasure truckMeasure)
		{
			this.truckMeasure = truckMeasure;
		}

		TruckMeasure truckMeasure;
		/// <summary>
		/// 汽车测量数据
		/// </summary>
		public TruckMeasure TruckMeasure
		{
			get { return truckMeasure; }
		}

		int minGranularity = 30;
		/// <summary>
		/// 接触最小间隔尺寸
		/// </summary>
		public int MinGranularity
		{
			get { return minGranularity; }
			set { minGranularity = value; }
		}

		int aiguilleRadius = 15;
		/// <summary>
		/// 钻头半径
		/// </summary>
		public int AiguilleRadius
		{
			get { return aiguilleRadius; }
			set { aiguilleRadius = value; }
		}

		/// <summary>
		/// 总接触间隔
		/// </summary>
		int TotalGranularity
		{
			get { return this.minGranularity + this.aiguilleRadius; }
		}

		/// <summary>
		/// 获取车体预览图
		/// </summary>
		/// <param name="imageWidth">图片宽度</param>
		/// <param name="imageHeight">图片高度</param>
		/// <param name="points">坐标点</param>
		/// <returns></returns>
		public Bitmap GetPreviewBitmap(int imageWidth, int imageHeight, List<Point> points, bool Rotate180FlipNone = false, ePointStyle pointStyle = ePointStyle.Point)
		{
			Bitmap res = new Bitmap(imageWidth, imageHeight);
			if (!Rotate180FlipNone)
			{

				Graphics g = Graphics.FromImage(res);
				// 填充背景
				g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#F7F7F7")), new Rectangle(0, 0, res.Width, res.Height));

				// 边距
				float padding = imageWidth * 0.02f;
				// 整体缩放比例
				float zoomRate = Math.Min((imageWidth - padding * 2) / this.truckMeasure.TruckTotalLength, (imageHeight - padding * 2 - 40) / this.truckMeasure.CarriageWidth);
				// 汽车总长
				float truckTotalLength = this.truckMeasure.TruckTotalLength * zoomRate;
				// 车厢宽
				float carriageWidth = this.truckMeasure.CarriageWidth * zoomRate;
				// 车头长
				float truckHeadLength = this.truckMeasure.TruckHeadLength * zoomRate;
				// 车厢长
				float carriageLength = this.truckMeasure.CarriageLength * zoomRate;
				// 车厢尾部到第1根拉筋距离
				float fromTailObstacle1 = this.truckMeasure.FromTailObstacle1 * zoomRate;
				// 车厢尾部到第2根拉筋距离
				float fromTailObstacle2 = this.truckMeasure.FromTailObstacle2 * zoomRate;
				// 车厢尾部到第3根拉筋距离
				float fromTailObstacle3 = this.truckMeasure.FromTailObstacle3 * zoomRate;
				// 车厢尾部到第4根拉筋距离
				float fromTailObstacle4 = this.truckMeasure.FromTailObstacle4 * zoomRate;
				// 车厢尾部到第5根拉筋距离
				float fromTailObstacle5 = this.truckMeasure.FromTailObstacle5 * zoomRate;
				// 车厢尾部到第6根拉筋距离
				float fromTailObstacle6 = this.truckMeasure.FromTailObstacle6 * zoomRate;

				// x轴位移
				float xOffest = (imageWidth - padding * 2 - truckTotalLength) / 2f;
				// y轴位移
				float yOffest = (imageHeight - padding * 2 - carriageWidth) / 2f;

				// 绘制车头
				float truckHeadDecrement = Math.Max(2.5f, carriageWidth * 0.02f);
				g.DrawRectangle(Pens.RoyalBlue, padding + xOffest, padding + yOffest + truckHeadDecrement, truckHeadLength - truckHeadDecrement * 2, carriageWidth - truckHeadDecrement * 2);
				g.FillRectangle(Brushes.RoyalBlue, padding + xOffest, padding + yOffest + truckHeadDecrement, truckHeadLength - truckHeadDecrement * 2, carriageWidth - truckHeadDecrement * 2);
				// 绘制车厢
				g.DrawRectangle(new Pen(Color.RoyalBlue, 4), padding + xOffest + truckHeadLength, padding + yOffest, carriageLength, carriageWidth);
				g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#D5D5D5")), padding + xOffest + truckHeadLength, padding + yOffest, carriageLength, carriageWidth);
				// 绘制拉筋
				Pen obstaclePen = new Pen(Color.Red, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
				Font obstacleFont = new Font("微软雅黑", 8, FontStyle.Regular);
				if (this.truckMeasure.Obstacle1 > 0)
				{
					g.DrawString("拉筋一", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle1 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle1.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle1 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle1, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle1, padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle2 > 0)
				{
					g.DrawString("拉筋二", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle2 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle2.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle2 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle2, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle2, padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle3 > 0)
				{
					g.DrawString("拉筋三", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle3 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle3.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle3 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle3, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle3, padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle4 > 0)
				{
					g.DrawString("拉筋四", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle4 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle4.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle4 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle4, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle4, padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle5 > 0)
				{
					g.DrawString("拉筋五", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle5 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle5.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle5 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle5, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle5, padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle6 > 0)
				{
					g.DrawString("拉筋六", obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle6 - 18, padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle6.ToString(), obstacleFont, Brushes.Blue, padding + xOffest + truckTotalLength - fromTailObstacle6 - 11, padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, padding + xOffest + truckTotalLength - fromTailObstacle6, padding + yOffest, padding + xOffest + truckTotalLength - fromTailObstacle6, padding + carriageWidth + yOffest);
				}

				// 绘制坐标点
				if (pointStyle == ePointStyle.Point)
				{
					float pointSize = (float)Math.Floor(Math.Max(2, this.AiguilleRadius * zoomRate));
					foreach (Point point in points) g.FillRectangle(Brushes.Green, padding + xOffest + truckTotalLength - point.X * zoomRate - pointSize, point.Y * zoomRate + yOffest - pointSize, 2 * pointSize, 2 * pointSize);
				}
				else if (pointStyle == ePointStyle.Number)
				{
					Font pointFont = new Font("微软雅黑", (float)Math.Floor(Math.Max(6, this.AiguilleRadius * zoomRate)), FontStyle.Regular);
					for (int i = 0; i < points.Count; i++) g.DrawString((i + 1).ToString(), pointFont, Brushes.Black, padding + xOffest + truckTotalLength - points[i].X * zoomRate, points[i].Y * zoomRate + yOffest);
				}
			}
			else
			{

				Graphics g = Graphics.FromImage(res);
				// 填充背景
				g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#F7F7F7")), new Rectangle(0, 0, res.Width, res.Height));

				// 边距
				float padding = imageWidth * 0.02f;
				// 整体缩放比例
				float zoomRate = Math.Min((imageWidth - padding * 2) / this.truckMeasure.TruckTotalLength, (imageHeight - padding * 2 - 40) / this.truckMeasure.CarriageWidth);
				// 汽车总长
				float truckTotalLength = this.truckMeasure.TruckTotalLength * zoomRate;
				// 车厢宽
				float carriageWidth = this.truckMeasure.CarriageWidth * zoomRate;
				// 车头长
				float truckHeadLength = this.truckMeasure.TruckHeadLength * zoomRate;
				// 车厢长
				float carriageLength = this.truckMeasure.CarriageLength * zoomRate;
				// 车厢尾部到第1根拉筋距离
				float fromTailObstacle1 = this.truckMeasure.FromTailObstacle1 * zoomRate;
				// 车厢尾部到第2根拉筋距离
				float fromTailObstacle2 = this.truckMeasure.FromTailObstacle2 * zoomRate;
				// 车厢尾部到第3根拉筋距离
				float fromTailObstacle3 = this.truckMeasure.FromTailObstacle3 * zoomRate;
				// 车厢尾部到第4根拉筋距离
				float fromTailObstacle4 = this.truckMeasure.FromTailObstacle4 * zoomRate;
				// 车厢尾部到第5根拉筋距离
				float fromTailObstacle5 = this.truckMeasure.FromTailObstacle5 * zoomRate;
				// 车厢尾部到第6根拉筋距离
				float fromTailObstacle6 = this.truckMeasure.FromTailObstacle6 * zoomRate;

				// x轴位移
				float xOffest = (imageWidth - padding * 2 - truckTotalLength) / 2f;
				// y轴位移
				float yOffest = (imageHeight - padding * 2 - carriageWidth) / 2f;

				// 绘制车头
				float truckHeadDecrement = Math.Max(2.5f, carriageWidth * 0.02f);
				g.DrawRectangle(Pens.RoyalBlue, imageWidth - (padding + xOffest) - (truckHeadLength - truckHeadDecrement * 2), imageHeight - (padding + yOffest + truckHeadDecrement) - (carriageWidth - truckHeadDecrement * 2), truckHeadLength - truckHeadDecrement * 2, carriageWidth - truckHeadDecrement * 2);
				g.FillRectangle(Brushes.RoyalBlue, imageWidth - (padding + xOffest) - (truckHeadLength - truckHeadDecrement * 2), imageHeight - (padding + yOffest + truckHeadDecrement) - (carriageWidth - truckHeadDecrement * 2), truckHeadLength - truckHeadDecrement * 2, carriageWidth - truckHeadDecrement * 2);
				// 绘制车厢
				g.DrawRectangle(new Pen(Color.RoyalBlue, 4), imageWidth - (padding + xOffest + truckHeadLength) - carriageLength, imageHeight - (padding + yOffest) - carriageWidth, carriageLength, carriageWidth);
				g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#D5D5D5")), imageWidth - (padding + xOffest + truckHeadLength) - carriageLength, imageHeight - (padding + yOffest) - carriageWidth, carriageLength, carriageWidth);
				// 绘制拉筋
				Pen obstaclePen = new Pen(Color.Red, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
				Font obstacleFont = new Font("微软雅黑", 8, FontStyle.Regular);
				if (this.truckMeasure.Obstacle1 > 0)
				{
					g.DrawString("拉筋一", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle1 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle1.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle1 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle1), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle1), padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle2 > 0)
				{
					g.DrawString("拉筋二", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle2 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle2.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle2 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle2), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle2), padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle3 > 0)
				{
					g.DrawString("拉筋三", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle3 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle3.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle3 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle3), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle3), padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle4 > 0)
				{
					g.DrawString("拉筋四", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle4 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle4.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle4 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle4), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle4), padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle5 > 0)
				{
					g.DrawString("拉筋五", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle5 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle5.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle5 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle5), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle5), padding + carriageWidth + yOffest);
				}
				if (this.truckMeasure.Obstacle6 > 0)
				{
					g.DrawString("拉筋六", obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle6 + 18), padding + yOffest - 20);
					g.DrawString(this.truckMeasure.FromTailObstacle6.ToString(), obstacleFont, Brushes.Blue, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle6 + 11), padding + carriageWidth + yOffest + 5);
					g.DrawLine(obstaclePen, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle6), padding + yOffest, imageWidth - (padding + xOffest + truckTotalLength - fromTailObstacle6), padding + carriageWidth + yOffest);
				}

				// 绘制坐标点
				if (pointStyle == ePointStyle.Point)
				{
					float pointSize = (float)Math.Floor(Math.Max(2, this.AiguilleRadius * zoomRate));
					foreach (Point point in points) g.FillRectangle(Brushes.Green, imageWidth - (padding + xOffest + truckTotalLength - point.X * zoomRate - pointSize) - (2 * pointSize), imageHeight - (point.Y * zoomRate + yOffest - pointSize) - (2 * pointSize), 2 * pointSize, 2 * pointSize);
				}
				else if (pointStyle == ePointStyle.Number)
				{
					Font pointFont = new Font("微软雅黑", (float)Math.Floor(Math.Max(6, this.AiguilleRadius * zoomRate)), FontStyle.Regular);
					for (int i = 0; i < points.Count; i++) g.DrawString((i + 1).ToString(), pointFont, Brushes.Black, padding + xOffest + truckTotalLength - points[i].X * zoomRate, points[i].Y * zoomRate + yOffest);
				}
			}

			return res;
		}

		/// <summary>
		/// 获取车体预览图
		/// </summary>
		/// <param name="imageWidth">图片宽度</param>
		/// <param name="imageHeight">图片高度</param>
		/// <param name="points">坐标点</param>
		/// <returns></returns>
		public Bitmap GetPreviewBitmap(int imageWidth, int imageHeight, ePointStyle pointStyle = ePointStyle.Number)
		{
			return GetPreviewBitmap(imageWidth, imageHeight, new List<Point>(), false, pointStyle);
		}

		/// <summary>
		/// 生成车厢采样坐标
		/// </summary>
		/// <param name="pointCount">坐标个数</param>
		/// <param name="pointBuildMode">生成模式</param>
		/// <returns></returns>
		public List<Point> GetPoints(int pointCount, ePointBuildMode pointBuildMode)
		{
			int x = 0, y = 0;
			List<Point> res = new List<Point>();

			do
			{
				res.Clear();

				if (pointBuildMode == ePointBuildMode.Random)
				{
					for (int i = 0; i < pointCount; i++)
					{
						x = BuildEffectivePoint(() => TruckCarriagePositioner.StaticRandom.Next(0, this.truckMeasure.CarriageLength), IsTouchX);
						y = BuildEffectivePoint(() => TruckCarriagePositioner.StaticRandom.Next(0, this.truckMeasure.CarriageWidth), IsTouchY);
						if (x > 0 && y > 0) res.Add(new Point(x, y));
					}
				}
				else if (pointBuildMode == ePointBuildMode.Partition)
				{
					int partLength = this.truckMeasure.CarriageLength / pointCount;
					for (int i = 0; i < pointCount; i++)
					{
						x = BuildEffectivePoint(() => TruckCarriagePositioner.StaticRandom.Next(i * partLength, (i + 1) * partLength), IsTouchX);
						y = BuildEffectivePoint(() => TruckCarriagePositioner.StaticRandom.Next(0, this.truckMeasure.CarriageWidth), IsTouchY);
						if (x > 0 && y > 0) res.Add(new Point(x, y));
					}
				}
			} while (HasPointTouch(res));

			return res;
		}

		/// <summary>
		/// 生成有效的坐标
		/// </summary>
		/// <param name="funcBuild"></param>
		/// <param name="funcCheck"></param>
		/// <returns></returns>
		public int BuildEffectivePoint(Func<int> funcBuild, Func<int, bool> funcCheck)
		{
			int res = -1;
			for (int i = 0; i < 100; i++)
			{
				res = funcBuild();
				if (!funcCheck(res)) return res; else res = -1;
			}

			return res;
		}

		/// <summary>
		/// 判断x轴坐标点是否与车厢或拉筋接触
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public bool IsTouchX(int x)
		{
			// 车厢前端
			if (Math.Abs(x - this.truckMeasure.CarriageLength) <= this.TotalGranularity) return true;
			// 第1根拉筋
			if (this.truckMeasure.Obstacle1 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle1) <= this.TotalGranularity) return true;
			// 第2根拉筋
			if (this.truckMeasure.Obstacle2 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle2) <= this.TotalGranularity) return true;
			// 第3根拉筋
			if (this.truckMeasure.Obstacle3 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle3) <= this.TotalGranularity) return true;
			// 第4根拉筋
			if (this.truckMeasure.Obstacle4 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle4) <= this.TotalGranularity) return true;
			// 第5根拉筋
			if (this.truckMeasure.Obstacle5 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle5) <= this.TotalGranularity) return true;
			// 第6根拉筋
			if (this.truckMeasure.Obstacle6 > 0 && Math.Abs(x - this.truckMeasure.FromTailObstacle6) <= this.TotalGranularity) return true;
			// 车厢尾端
			if (x <= this.TotalGranularity) return true;

			return false;
		}

		/// <summary>
		/// 判断y轴坐标点是否与车厢或拉筋接触
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsTouchY(int y)
		{
			// 车厢右侧
			if (y <= this.TotalGranularity) return true;
			// 车厢左侧
			if (Math.Abs(y - this.truckMeasure.CarriageWidth) <= this.TotalGranularity) return true;

			return false;
		}

		/// <summary>
		/// 判断坐标之间是否有重叠
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public bool HasPointTouch(List<Point> points)
		{
			for (int i = 0; i < points.Count; i++)
			{
				for (int j = i + 1; j < points.Count; j++)
				{
					if (Math.Abs(points[i].X - points[j].X) <= this.TotalGranularity && Math.Abs(points[i].Y - points[j].Y) <= this.TotalGranularity) return true;
				}
			}

			return false;
		}
	}

	/// <summary>
	/// 火车车厢采样定位类
	/// </summary>
	public class TrainCarriagePositioner
	{
		static Random StaticRandom = new Random();

		public TrainCarriagePositioner(TrainMeasure trarinMeasure)
		{
			this.trainMeasure = trarinMeasure;
		}

		TrainMeasure trainMeasure;
		/// <summary>
		/// 火车测量数据
		/// </summary>
		public TrainMeasure TrainMeasure
		{
			get { return trainMeasure; }
		}

		int minGranularity = 40;
		/// <summary>
		/// 接触最小间隔尺寸
		/// </summary>
		public int MinGranularity
		{
			get { return minGranularity; }
			set { minGranularity = value; }
		}

		int aiguilleRadius = 15;
		/// <summary>
		/// 钻头半径
		/// </summary>
		public int AiguilleRadius
		{
			get { return aiguilleRadius; }
			set { aiguilleRadius = value; }
		}

		/// <summary>
		/// 总接触间隔
		/// </summary>
		int TotalGranularity
		{
			get { return this.minGranularity + this.aiguilleRadius; }
		}

		/// <summary>
		/// 获取车体预览图
		/// </summary>
		/// <param name="imageWidth">图片宽度</param>
		/// <param name="imageHeight">图片高度</param>
		/// <param name="points">坐标点</param>
		/// <returns></returns>
		public Bitmap GetPreviewBitmap(int imageWidth, int imageHeight, List<Point> points, ePointStyle pointStyle = ePointStyle.Number)
		{
			Bitmap res = new Bitmap(imageWidth, imageHeight);

			Graphics g = Graphics.FromImage(res);
			// 填充背景
			g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#F7F7F7")), new Rectangle(0, 0, res.Width, res.Height));

			// 边距
			float padding = imageWidth * 0.02f;
			// 整体缩放比例
			float zoomRate = (imageWidth - padding * 2) / this.trainMeasure.CarriageLength;
			// 车厢宽
			float carriageWidth = this.trainMeasure.CarriageWidth * zoomRate;
			// 车厢长度
			float carriageLength = this.trainMeasure.CarriageLength * zoomRate;

			// y轴位移
			float yOffest = (imageHeight - padding * 2 - carriageWidth) / 2f;

			// 绘制车厢
			g.DrawRectangle(new Pen(Color.RoyalBlue, 4), padding, padding + yOffest, carriageLength, carriageWidth);
			g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#D5D5D5")), padding, padding + yOffest, carriageLength, carriageWidth);

			// 绘制坐标点
			if (pointStyle == ePointStyle.Point)
			{
				float pointSize = (float)Math.Floor(Math.Max(2, this.AiguilleRadius * zoomRate));
				foreach (Point point in points) g.FillRectangle(Brushes.Green, padding + point.X * zoomRate, point.Y * zoomRate + yOffest, pointSize, pointSize);
			}
			else if (pointStyle == ePointStyle.Number)
			{
				Font pointFont = new Font("微软雅黑", (float)Math.Floor(Math.Max(6, this.AiguilleRadius * zoomRate)), FontStyle.Regular);
				for (int i = 0; i < points.Count; i++) g.DrawString((i + 1).ToString(), pointFont, Brushes.Black, padding + points[i].X * zoomRate, points[i].Y * zoomRate + yOffest);
			}

			return res;
		}

		/// <summary>
		/// 获取车体预览图
		/// </summary>
		/// <param name="imageWidth">图片宽度</param>
		/// <param name="imageHeight">图片高度</param>
		/// <param name="points">坐标点</param>
		/// <returns></returns>
		public Bitmap GetPreviewBitmap(int imageWidth, int imageHeight, ePointStyle pointStyle = ePointStyle.Number)
		{
			return GetPreviewBitmap(imageWidth, imageHeight, new List<Point>(), pointStyle);
		}

		/// <summary>
		/// 生成车厢采样坐标
		/// </summary>
		/// <param name="pointCount">坐标个数</param>
		/// <param name="pointBuildMode">生成模式</param>
		/// <returns></returns>
		public List<Point> GetPoints(int pointCount, ePointBuildMode pointBuildMode)
		{
			int x = 0, y = 0;
			List<Point> res = new List<Point>();

			do
			{
				res.Clear();

				if (pointBuildMode == ePointBuildMode.Random)
				{
					for (int i = 0; i < pointCount; i++)
					{
						x = BuildEffectivePoint(() => TrainCarriagePositioner.StaticRandom.Next(0, this.trainMeasure.CarriageLength), IsTouchX);
						y = BuildEffectivePoint(() => TrainCarriagePositioner.StaticRandom.Next(0, this.trainMeasure.CarriageWidth), IsTouchY);
						if (x > 0 && y > 0) res.Add(new Point(x, y));
					}
				}
				else if (pointBuildMode == ePointBuildMode.Partition)
				{
					int partLength = this.trainMeasure.CarriageLength / pointCount;
					for (int i = 0; i < pointCount; i++)
					{
						x = BuildEffectivePoint(() => TrainCarriagePositioner.StaticRandom.Next(i * partLength, +(i + 1) * partLength), IsTouchX);
						y = BuildEffectivePoint(() => TrainCarriagePositioner.StaticRandom.Next(0, this.trainMeasure.CarriageWidth), IsTouchY);

						if (x > 0 && y > 0) res.Add(new Point(x, y));
					}
				}
			} while (HasPointTouch(res));

			return res;
		}

		/// <summary>
		/// 生成有效的坐标
		/// </summary>
		/// <param name="funcBuild"></param>
		/// <param name="funcCheck"></param>
		/// <returns></returns>
		public int BuildEffectivePoint(Func<int> funcBuild, Func<int, bool> funcCheck)
		{
			int res = -1;
			for (int i = 0; i < 1000; i++)
			{
				res = funcBuild();
				if (!funcCheck(res)) return res; else res = -1;
			}

			return res;
		}

		/// <summary>
		/// 判断x轴坐标点是否与车厢或拉筋接触
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public bool IsTouchX(int x)
		{
			// 车厢尾部
			if (x <= this.TotalGranularity) return true;
			// 车尾
			if (this.trainMeasure.CarriageLength - x <= this.TotalGranularity) return true;

			return false;
		}

		/// <summary>
		/// 判断y轴坐标点是否与车厢或拉筋接触
		/// </summary>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsTouchY(int y)
		{
			// 车厢右侧
			if (y <= this.TotalGranularity) return true;
			// 车厢左侧
			if (Math.Abs(y - this.trainMeasure.CarriageWidth) <= this.TotalGranularity) return true;

			return false;
		}

		/// <summary>
		/// 判断坐标之间是否有重叠
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public bool HasPointTouch(List<Point> points)
		{
			for (int i = 0; i < points.Count; i++)
			{
				for (int j = i + 1; j < points.Count; j++)
				{
					if (Math.Abs(points[i].X - points[j].X) <= this.TotalGranularity && Math.Abs(points[i].Y - points[j].Y) <= this.TotalGranularity) return true;
				}
			}

			return false;
		}
	}

	/// <summary>
	/// 汽车测量数据 单位：厘米
	/// </summary>
	public class TruckMeasure
	{
		/// <summary>
		/// TruckMeasure
		/// </summary>
		/// <param name="truckHeadLength">车头长</param>
		/// <param name="carriageWidth">车厢宽</param>
		/// <param name="carriageLength">车厢长</param>
		public TruckMeasure(int truckHeadLength, int carriageWidth, int carriageLength)
		{
			this.truckHeadLength = truckHeadLength;
			this.carriageWidth = carriageWidth;
			this.carriageLength = carriageLength;
		}

		/// <summary>
		/// 汽车总长
		/// </summary>
		public int TruckTotalLength
		{
			get { return truckHeadLength + carriageLength; }
		}

		private int truckHeadLength = 0;
		/// <summary>
		/// 车头长
		/// </summary>
		public int TruckHeadLength
		{
			get { return truckHeadLength; }
			set { truckHeadLength = value; }
		}

		private int carriageLength = 0;
		/// <summary>
		/// 车厢长
		/// </summary>
		public int CarriageLength
		{
			get { return carriageLength; }
			set { carriageLength = value; }
		}

		private int carriageWidth = 0;
		/// <summary>
		/// 车厢宽
		/// </summary>
		public int CarriageWidth
		{
			get { return carriageWidth; }
			set { carriageWidth = value; }
		}

		private int carriageToFloor = 0;
		/// <summary>
		/// 车厢底部距地面高
		/// </summary>
		public int CarriageToFloor
		{
			get { return carriageToFloor; }
			set { carriageToFloor = value; }
		}

		private int obstacle1 = 0;
		/// <summary>
		/// 车厢尾端到第1根拉筋距离（厘米）
		/// </summary>
		public int Obstacle1
		{
			get { return obstacle1; }
			set { obstacle1 = value; }
		}

		private int obstacle2 = 0;
		/// <summary>
		/// 车厢尾端第1根到第2根拉筋距离（厘米）
		/// </summary>
		public int Obstacle2
		{
			get { return obstacle2; }
			set { obstacle2 = value; }
		}

		private int obstacle3 = 0;
		/// <summary>
		/// 车厢尾端第2根到第3根拉筋距离（厘米）
		/// </summary>
		public int Obstacle3
		{
			get { return obstacle3; }
			set { obstacle3 = value; }
		}

		private int obstacle4 = 0;
		/// <summary>
		/// 车厢尾端第3根到第4根拉筋距离（厘米）
		/// </summary>
		public int Obstacle4
		{
			get { return obstacle4; }
			set { obstacle4 = value; }
		}

		private int obstacle5 = 0;
		/// <summary>
		/// 车厢尾端第4根到第5根拉筋距离（厘米）
		/// </summary>
		public int Obstacle5
		{
			get { return obstacle5; }
			set { obstacle5 = value; }
		}

		private int obstacle6 = 0;
		/// <summary>
		/// 车厢尾端第5根到第6根拉筋距离（厘米）
		/// </summary>
		public int Obstacle6
		{
			get { return obstacle6; }
			set { obstacle6 = value; }
		}

		/// <summary>
		/// 车厢尾部到第1根拉筋距离
		/// </summary>
		public int FromTailObstacle1
		{
			get { return obstacle1; }
		}

		/// <summary>
		/// 车厢尾部到第2根拉筋距离
		/// </summary>
		public int FromTailObstacle2
		{
			get { return FromTailObstacle1 + obstacle2; }
		}

		/// <summary>
		/// 车厢尾部到第3根拉筋距离
		/// </summary>
		public int FromTailObstacle3
		{
			get { return FromTailObstacle2 + obstacle3; }
		}

		/// <summary>
		/// 车厢尾部到第4根拉筋距离
		/// </summary>
		public int FromTailObstacle4
		{
			get { return FromTailObstacle3 + obstacle4; }
		}

		/// <summary>
		/// 车厢尾部到第5根拉筋距离
		/// </summary>
		public int FromTailObstacle5
		{
			get { return FromTailObstacle4 + obstacle5; }
		}

		/// <summary>
		/// 车厢尾部到第6根拉筋距离
		/// </summary>
		public int FromTailObstacle6
		{
			get { return FromTailObstacle5 + obstacle6; }
		}
	}

	/// <summary>
	/// 火车测量数据 单位：厘米
	/// </summary>
	public class TrainMeasure
	{
		/// <summary>
		/// TrainMeasure
		/// </summary> 
		/// <param name="carriageWidth">车厢宽</param>
		/// <param name="carriageLength">车厢长</param>
		public TrainMeasure(int carriageWidth, int carriageLength)
		{
			this.carriageWidth = carriageWidth;
			this.carriageLength = carriageLength;
		}

		private int carriageLength = 0;
		/// <summary>
		/// 车厢长
		/// </summary>
		public int CarriageLength
		{
			get { return carriageLength; }
			set { carriageLength = value; }
		}

		private int carriageWidth = 0;
		/// <summary>
		/// 车厢宽
		/// </summary>
		public int CarriageWidth
		{
			get { return carriageWidth; }
			set { carriageWidth = value; }
		}

		private int carriageToFloor = 0;
		/// <summary>
		/// 车厢底部距地面高
		/// </summary>
		public int CarriageToFloor
		{
			get { return carriageToFloor; }
			set { carriageToFloor = value; }
		}
	}

	/// <summary>
	/// 坐标点生成模式
	/// </summary>
	public enum ePointBuildMode
	{
		/// <summary>
		/// 随机
		/// </summary>
		Random = 0,
		/// <summary>
		/// 分块
		/// </summary>
		Partition = 1
	}

	/// <summary>
	/// 坐标点样式
	/// </summary>
	public enum ePointStyle
	{
		/// <summary>
		/// 数字
		/// </summary>
		Number = 0,
		/// <summary>
		/// 点
		/// </summary>
		Point = 1
	}
}
