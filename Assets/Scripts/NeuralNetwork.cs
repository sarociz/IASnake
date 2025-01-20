using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    private int inputSize, hiddenSize, outputSize;
    private float[,] weightsInputHidden;
    private float[,] weightsHiddenOutput;
    private List<Experience> memory;
    private float learningRate = 0.01f;
    private float discountFactor = 0.95f; // Importancia del futuro 




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
        System.Random random = new System.Random();
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                weightsInputHidden[i, j] = (float)random.NextDouble() * 2 - 1;
            }
        }

        for (int i = 0; i < hiddenSize; i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                weightsHiddenOutput[i, j] = (float)random.NextDouble() * 2 - 1;
            }
        }
    }

    // Método para predecir el mejor movimiento para la snake
    public int Predict(float[] inputs)
    {
        // Similar al código anterior (forward pass)
        float[] hidden = new float[hiddenSize];
        for (int i = 0; i < hiddenSize; i++)
        {
            hidden[i] = 0;
            for (int j = 0; j < inputSize; j++)
                hidden[i] += inputs[j] * weightsInputHidden[j, i];
            hidden[i] = Mathf.Tan(hidden[i]);
        }

        float[] outputs = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            outputs[i] = 0;
            for (int j = 0; j < hiddenSize; j++)
                outputs[i] += hidden[j] * weightsHiddenOutput[j, i];
        }

        // Devuelve la acción con mayor valor
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
        // Entrena usando las experiencias almacenadas
        // Agrega la experiencia al historial
        memory.Add(new Experience(state, action, reward, nextState));
        Train();
    }

    private void Train()
    {
        foreach (var experience in memory)
        {
            // Predicción actual (Q(s, a))
            float[] hidden = ForwardPass(experience.State, weightsInputHidden);
            float[] outputs = ForwardPass(hidden, weightsHiddenOutput);

            // Calcula el valor objetivo (Q-target)
            float qTarget = experience.Reward;
            if (experience.NextState != null)
            {
                // Busca el mejor Q(s', a') futuro si no es un estado terminal
                float[] nextHidden = ForwardPass(experience.NextState, weightsInputHidden);
                float[] nextOutputs = ForwardPass(nextHidden, weightsHiddenOutput);
                qTarget += discountFactor * Mathf.Max(nextOutputs);
            }

            // Calcula el error y ajusta los pesos
            float error = qTarget - outputs[experience.Action];
            Backpropagate(error, hidden, experience.Action);
        }


        memory.Clear();
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
            outputs[i] = Mathf.Tan(outputs[i]);
        }
        return outputs;
    }

    private void Backpropagate(float error, float[] hidden, int action)
    {
        // Ajusta los pesos de salida (hidden -> output)
        for (int i = 0; i < hiddenSize; i++)
        {
            float gradient = error * hidden[i];
            weightsHiddenOutput[i, action] += learningRate * gradient;
        }

        // (Opcional) Ajusta los pesos de entrada (input -> hidden) si lo necesitas
    }
}

// Clase para almacenar experiencias (estado, acción, recompensa, siguiente estado)
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
