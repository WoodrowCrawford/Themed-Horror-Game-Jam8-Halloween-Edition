using UnityEngine;

namespace MegaBook
{
	[ExecuteInEditMode]
	public class BookShelf : MonoBehaviour
	{
		public float					width		= 1.0f;
		public Vector3					lowScale	= Vector3.one;
		public Vector3					highScale	= Vector3.one;
		public Gradient					colors		= new Gradient();
		public Vector3					offset;
		public int						seed;
		public bool						update		= false;
		public GameObject[]				books;
		public bool						runs		= true;
		public int						minRun		= 2;
		public int						maxRun		= 8;
		public float					runChance	= 0.25f;
		static MaterialPropertyBlock	pblock;

		void Start()
		{
			update = true;
		}

		void Update()
		{
			if ( update )
			{
				if ( pblock == null )
					pblock = new MaterialPropertyBlock();

				while ( transform.childCount > 0 )
					DestroyImmediate(transform.GetChild(0).gameObject);

				update = false;
				Random.InitState(seed);

				float x = 0.0f - (width * 0.5f);

				int runcount = -1;
				int lastbook = 0;
				Vector3 lastscale = Vector3.one;
				Color lastcol = Color.white;

				while ( x < (width * 0.5f) )
				{
					if ( runcount < 0 )
					{
						if ( Random.value < runChance )
							runcount = Random.Range(minRun, maxRun);
					}

					int booki = lastbook;
					if ( runcount < 0 )
						booki = Random.Range(0, books.Length - 0);

					GameObject bo = Instantiate(books[booki]);

					bo.transform.SetParent(transform);
					bo.transform.localEulerAngles = new Vector3(180.0f, 0.0f, 90.0f);

					Vector3 scale = lastscale;

					if ( runcount < 0 )
					{
						scale.x = Mathf.Lerp(lowScale.x, highScale.x, Random.value);
						scale.y = Mathf.Lerp(lowScale.y, highScale.y, Random.value);
						scale.z = Mathf.Lerp(lowScale.z, highScale.z, Random.value);
					}

					bo.transform.localScale = scale;

					Color col = lastcol;
				
					if ( runcount < 0 )
						col = colors.Evaluate(Random.value);

					Vector3 lpos = Vector3.zero;
					lpos.x += x;	// get from book

					// want gaps and tilted books
					bo.transform.localPosition = lpos + offset;

					MeshRenderer mr = bo.GetComponent<MeshRenderer>();

					pblock.SetColor("_Color", col);
					mr.SetPropertyBlock(pblock);

					x += 0.06f * scale.y;

					lastbook = booki;
					lastcol = col;
					lastscale = scale;
					runcount--;
				}
			}
		}
	}
}