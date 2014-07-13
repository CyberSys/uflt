using UnityEngine;
using System.Collections;

namespace UFLT.MonoBehaviours
{
	[ExecuteInEditMode]
	public class SwitchNode : MonoBehaviour
	{
		#region Properties
		
		/// <summary>
		/// The index of the current Mask
		/// </summary>
		public int Index;
		
		/// <summary>
		/// Node masks
		/// </summary>
		public int[] Masks;

		private int current = -1;
		
		#endregion
		
		private void Start() {
			int mask = Masks [Index];
			for (int i=0;i<transform.childCount;i++) {
				int mask_bit = 1 << (i % 32);
				transform.GetChild(i).gameObject.SetActive((mask & mask_bit) != 0);
			}
			current = Index;
		}
		
		private void Update()
		{
			if (current != Index && Index >= 0 && Index < Masks.Length) {
				current = Index;
				int mask = Masks [Index];
				for (int i=0;i<transform.childCount;i++) {
					int mask_bit = 1 << (i % 32);
					transform.GetChild(i).gameObject.SetActive((mask & mask_bit) != 0);
				}
			}
		}
	}
}