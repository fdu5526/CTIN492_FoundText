using UnityEngine;

public class CSVParser : MonoBehaviour{

 	// read CSV and poop out beat array
	public static string[][] Parse (string path) {
		TextAsset csv = Resources.Load(path) as TextAsset;

		string[] rows = csv.text.Split('\n');

		// Length - 2 to exclude Beat and Duration columns
		string[][] retval = new string[rows.Length - 1][];

		// read each row
		for(int i = 1; i < retval.Length + 1; i++) {
			retval[i-1] = rows[i].Split(',');
		}

		return retval;
	}
}