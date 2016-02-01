using UnityEngine;

public class CSVParser : MonoBehaviour{

 	// read CSV and poop out beat array
	public static char[][] Parse (string path) {
		TextAsset csv = Resources.Load(path) as TextAsset;

		string[] rows = csv.text.Split('\n');

		// Length - 2 to exclude Beat and Duration columns
		char[][] retval = new char[rows.Length - 1][];

		// read each row
		for(int i = 1; i < retval.Length + 1; i++) {
			string[] data = rows[i].Split(',');
			retval[i-1] = new char [data.Length];
			for (int d = 0; d < data.Length; d++) {
				retval[i-1][d] = data[d][0];
			}
			
		}

		return retval;
	}
}