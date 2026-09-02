using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StageLevels", menuName = "Scene Management/StageLevels")]
public class StageLevels : ScriptableObject
{
    public string[] levels;

    public bool ContainsLevel(string levelName)
    {
        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i] == levelName) return true;
        }

        return false;
    }

    public bool NextLevel(out string nextLevel)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        for (int i = 0; i < levels.Length - 1; ++i)
        {
            if (levels[i] != currentLevel) continue;
            
            nextLevel = levels[i + 1];
            return true;
        }

        nextLevel = "";
        return false;
    }

    public static bool GetNextLevel(StageLevels[] stages, out string nextLevelName)
    {
        StageLevels currentStage = GetCurrentStage(stages, out int stageIndex);

        if (currentStage.NextLevel(out string result))
        {
            nextLevelName = result;
            return true;
        }

        int nextStageIndex = stageIndex + 1;

        if (nextStageIndex >= stages.Length)
        {
            nextLevelName = "";
            return false;
        }

        nextLevelName = stages[nextStageIndex].levels[0];
        return true;
    }

    public static StageLevels GetCurrentStage(StageLevels[] stages, out int stageIndex)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        for (int i = 0; i < stages.Length; ++i)
        {
            if (!stages[i].ContainsLevel(currentLevel)) continue;

            stageIndex = i;
            return stages[i];
        }

        stageIndex = -1;
        return null;
    }
}
