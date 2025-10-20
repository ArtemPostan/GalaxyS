using System; // ����� ��� [Serializable]
using UnityEngine;

// ��������� ������������� ����� � ����������
[Serializable]
public class LevelConfig
{
    [Tooltip("����������� ���������� Shape ��� ����� ������ ���������.")]
    public int minShapes = 3;

    [Tooltip("������������ ���������� Shape ��� ����� ������ ���������.")]
    public int maxShapes = 5;

    [Tooltip("������� �������� ��� ������� Shape.")]
    public int baseShapeHealth = 10;

    [Tooltip("��������� ��� �������� Shape (�������� = baseHealth * healthMultiplier).")]
    public float healthMultiplier = 1.0f;

    [Tooltip("��������, � ������� ���� ����� � ������.")]
    public float enemySpeed = 10f;

    [Tooltip("�������� ����� �������� �� ���� ������.")]
    public float spawnInterval = 1.5f;
}