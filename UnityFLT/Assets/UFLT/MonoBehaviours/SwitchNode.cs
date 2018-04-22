using UnityEngine;
using UFLT.Records;

namespace UFLT.MonoBehaviours
{
	[ExecuteInEditMode]
	public class SwitchNode : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// How often the switch should be checked.
		/// </summary>
		public static float switchUpdateRate = 0.1f;

		/// <summary>
		/// The index of the current Mask
		/// </summary>
		public int index;

		/// <summary>
		/// Node masks
		/// </summary>
		public int[] masks;

		private int _currentIndex = -1;

		#endregion

		/// <summary>
		/// Called by the Switch class when creating an OpenFlight Switch node from file.
		/// </summary>
		/// <param name="switchData"></param>
		public virtual void OnSwitchNode(Switch switchData)
		{
			index = switchData.Index;
			masks = switchData.Masks;
			InitSwitch();
		}

		private void Start()
		{
			if (masks != null)
				InitSwitch();
		}

		/// <summary>
		/// Setup the switch and start invoking.
		/// </summary>
		private void InitSwitch()
		{
			int mask = masks[index];
			for (int i = 0; i < transform.childCount; i++)
			{
				int mask_bit = 1 << (i % 32);
				transform.GetChild(i).gameObject.SetActive((mask & mask_bit) != 0);
			}
			_currentIndex = index;
			InvokeRepeating("UpdateSwitch", switchUpdateRate, switchUpdateRate);
		}

		/// <summary>
		/// Check if we need to perform a switch.
		/// </summary>
		private void UpdateSwitch()
		{
			if (_currentIndex != index && index >= 0 && index < masks.Length)
			{
				_currentIndex = index;
				int mask = masks[index];
				for (int i = 0; i < transform.childCount; i++)
				{
					int mask_bit = 1 << (i % 32);
					transform.GetChild(i).gameObject.SetActive((mask & mask_bit) != 0);
				}
			}
		}
	}
}