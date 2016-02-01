using UnityEngine;

public class CSVParser : MonoBehaviour{

 	// read CSV and poop out beat array
	public static string[][] Parse (string path) {
		TextAsset csv = Resources.Load(path) as TextAsset;

		string[] rows = csv.text.Split('\n');
		string[] labels = rows[0].Split(',');


		// Length - 2 to exclude Beat and Duration columns
		/*Beat[] beats = new Beat[rows.Length - 2];

		// read each row
		for(int i = 1; i < rows.Length - 1; i++) {
			string[] data = rows[i].Split(',');

			// get the dialogues
			Dialogue[] beatLines = new Dialogue[(data.Length - 2) / 2];
			for (int d = 0; d < beatLines.Length; d++) {
				int rd = d + 1;
				string a = data[2 * rd].Length == 0 ? null : data[2 * rd];
				string l = data[2 * rd + 1].Length == 0 ? null : data[2 * rd + 1];
				beatLines[d] = new Dialogue(a, l);
			}

			// make the beat
			float f = 0f;
			float.TryParse(data[1], out f);
			beats[i - 1] = new Beat(f, beatLines);
		}*/

		return null;
	}
}