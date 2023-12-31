namespace OpenCvSharp.Demo
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	using System;

	using OpenCvSharp;
	using OpenCvSharp.Tracking;
    using System.Linq;

    /// <summary>
    /// Object tracking handler
    /// </summary>
    public class TrackingScript
		: WebCamera
	{
		public GameController gameController;
		const float downScale = 0.33f;
		Mat frame;
		Rect2d objectRoi;
		Scalar lowerColorRed1 = new Scalar(0, 100, 100);
		Scalar upperColorRed1 = new Scalar(10, 255, 255);
		Scalar lowerColorRed2 = new Scalar(160, 100, 100);
		Scalar upperColorRed2 = new Scalar(180, 255, 255);
		Scalar lowerColorBlack = new Scalar(0, 0, 0);
		Scalar upperColorBlack = new Scalar(180, 255, 30);

		const double areaThreshold = 300;
		Vector2 startPoint = Vector2.zero;
		Vector2 endPoint = Vector2.zero;
		protected override void Awake()
		{
			base.Awake();
			forceFrontalCamera = true;
		}
		Vector2 ConvertToImageSpace(Vector2 coord, Size size)
		{
			var ri = GetComponent<UnityEngine.UI.RawImage>();

			Vector2 output = new Vector2();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(ri.rectTransform, coord, null, out output);

			output.x += size.Width / 2;
			output.y += size.Height / 2;

			if (!TextureParameters.FlipVertically)
				output.y = size.Height - output.y;
			
			output.x *= downScale;
			output.y *= downScale;

			return output;
		}

		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			Mat frame = Unity.TextureToMat(input, TextureParameters);
			Mat downscaled = frame.Resize(Size.Zero, downScale, downScale);

			Mat hsvRed = frame.CvtColor(ColorConversionCodes.BGR2HSV);
			Mat maskRed1 = hsvRed.InRange(lowerColorRed1, upperColorRed1);
			Mat maskRed2 = hsvRed.InRange(lowerColorRed2, upperColorRed2);
			Mat maskRed = new Mat();

			Cv2.BitwiseOr(maskRed1, maskRed2, maskRed);
			maskRed.Erode(null, iterations: 2);
			maskRed.Dilate(null, iterations: 2);

			

			Mat hsvBlack = frame.CvtColor(ColorConversionCodes.BGR2HSV);
			Mat maskBlack = hsvBlack.InRange(lowerColorBlack, upperColorBlack);

			maskBlack.Erode(null, iterations: 3);
			maskBlack.Dilate(null, iterations: 3);

			Point[][] redContours;
			Point[][] blackContours;
			HierarchyIndex[] redHierarchy;
			HierarchyIndex[] blackHierarchy;

			maskRed.FindContours(out redContours, out redHierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
			maskBlack.FindContours(out blackContours, out blackHierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
			
			if (redContours.Length > 0)
			{
				var maxContour = redContours.OrderByDescending(c => Cv2.ContourArea(c)).First();

				double area = Cv2.ContourArea(maxContour);

				if (area > areaThreshold)
				{
					var rect = Cv2.BoundingRect(maxContour);
					objectRoi = new Rect2d(rect.X, rect.Y, rect.Width, rect.Height);
					Point startPoint = new Point(objectRoi.X, objectRoi.Y);
					Point endPoint = new Point(objectRoi.X + objectRoi.Width, objectRoi.Y + objectRoi.Height);

					Cv2.Rectangle(frame, startPoint, endPoint, new Scalar(0, 255, 0), 5);

				}
			}
			else if (blackContours.Length > 0)
			{
				var maxContour = blackContours.OrderByDescending(c => Cv2.ContourArea(c)).First();

				double area = Cv2.ContourArea(maxContour);

				if (area > areaThreshold)
				{
					var rect = Cv2.BoundingRect(maxContour);
					objectRoi = new Rect2d(rect.X, rect.Y, rect.Width, rect.Height);
					Point startPoint = new Point(objectRoi.X, objectRoi.Y);
					Point endPoint = new Point(objectRoi.X + objectRoi.Width, objectRoi.Y + objectRoi.Height);

					Cv2.Rectangle(frame, startPoint, endPoint, new Scalar(0, 255, 0), 5);
				}

			}

			Vector2 sp = ConvertToImageSpace(startPoint, frame.Size());
			Vector2 ep = ConvertToImageSpace(endPoint, frame.Size());
			Point location = new Point(Math.Min(sp.x, ep.x), Math.Min(sp.y, ep.y));
			Size size = new Size(Math.Abs(ep.x - sp.x), Math.Abs(ep.y - sp.y));
			var areaRect = new OpenCvSharp.Rect(location, size);
			Rect2d obj = Rect2d.Empty;
			
			gameController.TrackingRacket((float)objectRoi.X, (float)objectRoi.Y, (float)objectRoi.Width, (float)objectRoi.Height, frame.Size().Width, frame.Size().Height);

			output = Unity.MatToTexture(frame, output);

			return true;
		}
	}
}