using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class MegaBookControl : MonoBehaviour
	{
		public MegaBookBuilder book;
		public UnityEngine.UI.Slider pageSlider;
		public UnityEngine.UI.Toggle snapToggle;

		void Start()
		{
			if (book)
			{
				if (pageSlider)
				{
					pageSlider.minValue = book.MinPageVal();
					pageSlider.maxValue = book.MaxPageVal();
					pageSlider.value = book.page;
				}

				if (snapToggle)
					snapToggle.isOn = book.Snap;
			}
		}

		public void SetTarget(MegaBookBuilder newbook)
		{
			if (newbook)
			{
				book = newbook;

				if (pageSlider)
				{
					pageSlider.minValue = book.MinPageVal();
					pageSlider.maxValue = book.MaxPageVal();
					pageSlider.value = book.page;
				}

				if (snapToggle)
					snapToggle.isOn = book.Snap;
			}
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				book.NextPage();

			if (Input.GetKeyDown(KeyCode.PageDown))
				book.PrevPage();
		}

		public void NextPage()
		{
			book.NextPage();
		}

		public void PrevPage()
		{
			book.PrevPage();
		}

		public void SetSnap(bool val)
		{
			book.Snap = val;
		}

		public void SetPage(float p)
		{
			if (book)
				book.page = p;
		}

		
	}
}