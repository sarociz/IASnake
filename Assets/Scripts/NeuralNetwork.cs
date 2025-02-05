using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    private int inputSize, hiddenSize, outputSize;
    private float[,] weightsInputHidden;
    private float[,] weightsHiddenOutput;
    private List<Experience> memory;
    private float learningRate = 0.01f;
    private float discountFactor = 0.95f; // Importancia del futuro
    private float epsilon = 0.1f; // Exploración ?-greedy
    private int memorySize = 500; // Límite de memoria para evitar sobrecarga
    private System.Random random = new System.Random();

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;
        memory = new List<Experience>();

        // Inicializar pesos aleatorios
        weightsInputHidden = new float[inputSize, hiddenSize];
        weightsHiddenOutput = new float[hiddenSize, outputSize];
        InitializeWeights();
    }

    private void InitializeWeights()
    {
        for (int i = 0; i < inputSize; i++)
            for (int j = 0; j < hiddenSize; j++)
                weightsInputHidden[i, j] = (float)random.NextDouble() * 2 - 1;

        for (int i = 0; i < hiddenSize; i++)
            for (int j = 0; j < outputSize; j++)
                weightsHiddenOutput[i, j] = (float)random.NextDouble() * 2 - 1;
    }

    public int Predict(float[] inputs)
    {
        // Explorar con probabilidad epsilon (?-greedy)
        if (random.NextDouble() < epsilon)
            return random.Next(outputSize); // Acción aleatoria

        // Forward pass
        float[] hidden = ForwardPass(inputs, weightsInputHidden);
        float[] outputs = ForwardPass(hidden, weightsHiddenOutput);

        // Seleccionar la mejor acción
        int bestAction = 0;
        float maxOutput = outputs[0];
        for (int i = 1; i < outputSize; i++)
        {
            if (outputs[i] > maxOutput)
            {
                maxOutput = outputs[i];
                bestAction = i;
            }
        }
        return bestAction;
    }

    public void ApplyReward(float reward, float[] state, int action, float[] nextState)
    {
        // Almacenar la experiencia
        memory.Add(new Experience(state, action, reward, nextState));
        if (memory.Count > memorySize) memory.RemoveAt(0); // Limitar tamaño de memoria

        // Entrenar en lotes
        if (memory.Count >= 32) Train();
    }

    private void Train()
    {
        // Seleccionar minibatch aleatorio
        List<Experience> batch = new List<Experience>();
        for (int i = 0; i < 32; i++)
            batch.Add(memory[random.Next(memory.Count)]);

        foreach (var experience in batch)
        {
            float[] hidden = ForwardPass(experience.State, weightsInputHidden);
            float[] outputs = ForwardPass(hidden, weightsHiddenOutput);

            float qTarget = experience.Reward;
            if (experience.NextState != null)
            {
                float[] nextHidden = ForwardPass(experience.NextState, weightsInputHidden);
                float[] nextOutputs = ForwardPass(nextHidden, weightsHiddenOutput);
                qTarget += discountFactor * Mathf.Max(nextOutputs); // Usar max(Q(s', a'))
            }

            float error = qTarget - outputs[experience.Action];

            // Actualizar pesos usando backpropagation
            Backpropagate(error, hidden, experience.Action, experience.State);
        }

        if (memory.Count > 500)
        {
            memory.RemoveAt(0); // Eliminar la experiencia más antigua
        }
    }

    private float[] ForwardPass(float[] inputs, float[,] weights)
    {
        int outputSize = weights.GetLength(1);
        float[] outputs = new float[outputSize];

        for (int i = 0; i < outputSize; i++)
        {
            outputs[i] = 0;
            for (int j = 0; j < inputs.Length; j++)
                outputs[i] += inputs[j] * weights[j, i];

            // Usar ReLU en lugar de tangente
            outputs[i] = Mathf.Max(0, outputs[i]);
        }
        return outputs;
    }

    private void Backpropagate(float error, float[] hidden, int action, float[] inputs)
    {
        // Ajustar pesos hidden -> output
        for (int i = 0; i < hiddenSize; i++)
            weightsHiddenOutput[i, action] += learningRate * error * hidden[i];

        // Ajustar pesos input -> hidden
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                float gradient = error * weightsHiddenOutput[j, action] * ((hidden[j] > 0) ? 1 : 0); // Derivada de ReLU
                weightsInputHidden[i, j] += learningRate * gradient * inputs[i];
            }
        }
    }
}


public class Experience
{
    public float[] State;
    public int Action;
    public float Reward;
    public float[] NextState;

    public Experience(float[] state, int action, float reward, float[] nextState)
    {
        State = state;
        Action = action;
        Reward = reward;
        NextState = nextState;
    }
}
