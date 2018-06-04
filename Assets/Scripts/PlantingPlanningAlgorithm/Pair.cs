internal class Pair<T1, T2> {
	private int v1;
	private int v2;

	public Pair(int v1, int v2) {
		this.v1 = v1;
		this.v2 = v2;
	}

	public int getX() {
		return v1;
	}

	public int getY() {
		return v2;
	}
}