using System;
[Serializable]
public class NeuralNetwork {

	public int[] layer;
	public Layer[] layers;
	public static float LearningRate = 0.000000001f;  

	public NeuralNetwork(int [] layer) {
		this.layer = new int[layer.Length];
		for (int i = 0; i < layer.Length; i++) {
			this.layer [i] = layer [i];
		}

		layers = new Layer[layer.Length-1];

		for (int i = 0; i < layers.Length; i++) {
			layers [i] = new Layer (layer[i],layer[i+1]);
		}
	}
		

	public float[] FeedForward(float[]inputs) {
		layers [0].FeedForward (inputs);
		for (int i=1 ; i < layers.Length; i++) {
			layers [i].FeedForward (layers[i-1].outputs);
		}
		return layers[layers.Length-1].outputs;
	}

	public void BackPropagation(float [] expected){
		for (int i=layers.Length-1 ; i >= 0; i--) {
			if (i == layers.Length - 1) {
				layers [i].BackPropagationOutput (expected);
			} else {
				layers [i].BackPropagationHidden (layers[i+1].gamma,layers[i+1].weights);
			}
		}

		for (int i = 1; i < layers.Length; i++) {
			layers [i].UpdateWeights ();
		}
	}
	[Serializable]
	public class Layer
	{
		public int numberOfInputs; // number of neurons in prev layer
		public int numberOfOutputs; // current layer


		public float[] outputs;
		public float[] inputs;
		public float[,] weights;
		public float[,] weightsDelta;
		public float [] gamma;
		public float [] error;
		public static Random random = new Random();

		public Layer(int numberOfInputs, int numberOfOutputs) {
			this.numberOfInputs = numberOfInputs;
			this.numberOfOutputs = numberOfOutputs;

			outputs = new float[numberOfOutputs];
			inputs = new float[numberOfInputs];
			weights = new float[numberOfOutputs,numberOfInputs];
			weightsDelta = new float[numberOfOutputs,numberOfInputs];
			gamma = new float[numberOfOutputs];
			error = new float[numberOfOutputs];

			InitilizeWeights();
		}

		public void InitilizeWeights() {
			for (int i = 0; i < numberOfOutputs; i++) {
				for (int j = 0; j < numberOfInputs; j++) {
					weights [i, j] = (float)random.NextDouble () - 0.5f;
				}
			}
		}

		public float[] FeedForward(float [] inputs) {
			this.inputs = inputs;

			for (int i = 0; i < numberOfOutputs; i++) {
				outputs [i] = 0;
				for (int j = 0; j < numberOfInputs; j++) {
					outputs [i] += inputs [j] * weights [i, j];
				}
				outputs [i] = (float)Math.Tanh (outputs[i]);
			}

			return outputs;
		}
		public float TanHDerative(float value) {
			return 1- (value * value);
		}

		public void BackPropagationOutput(float [] expected) {
			for (int i = 0; i < numberOfOutputs; i++) {
				error [i] = outputs [i] - expected [i];
			}

			for (int i = 0; i < numberOfOutputs; i++) {
				gamma [i] = error [i] * TanHDerative (outputs[i]);
			}

			for (int i = 0; i < numberOfOutputs; i++) {
				for (int j = 0; j < numberOfInputs; j++) {
					weightsDelta [i, j] = gamma [i] * inputs [j];
				}
			}
		}

		public void BackPropagationHidden(float [] gammaForward , float [,] weightsForward) {
			for (int i = 0; i < numberOfOutputs; i++) {
				gamma [i] = 0;
				for (int j = 0; j < gammaForward.Length; j++) {
					gamma [i] += gammaForward [j] * weightsForward [j, i];
				}
				gamma [i] *= TanHDerative (outputs[i]);
			}
			for (int i = 0; i < numberOfOutputs; i++) {
				for (int j = 0; j < numberOfInputs; j++) {
					weightsDelta [i, j] = gamma [i] * inputs [j];
				}
			} 
		}

		public void UpdateWeights() {
			for (int i = 0; i < numberOfOutputs; i++) {
				for (int j = 0; j < numberOfInputs; j++) {
					weights [i, j] -= weightsDelta [i, j] * LearningRate;
				}
			}
		}
	}
}
