using Godot;
using System;

public partial class GameScore : Label
{
    public int score = 0;
    private float scoreTimer = 0f;
    [Export] public float ScoreRate = 1f;  
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		        scoreTimer += (float)delta;

        if (scoreTimer >= 1f / ScoreRate)
        {
            score += 1;
            scoreTimer = 0f;
            UpdateScoreLabel();
        }        scoreTimer += (float)delta;

        if (scoreTimer >= 1f / ScoreRate)
        {
            score += 1;
            scoreTimer = 0f;
            UpdateScoreLabel();
        }
	}

	private void UpdateScoreLabel()
    {
        Text = $"{score}";
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreLabel();
    }
}
