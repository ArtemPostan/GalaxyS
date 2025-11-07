using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaffChoiceManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameController gameController;
    [SerializeField] Button firstBuffbttn;
    [SerializeField] Button secondBuffbttn;
    [SerializeField] Button refuseBttn;
 
    PlayerStats playerStats;
   // [SerializeField] TextMeshProUGUI refusetext;

    //private Effects playerEffects;
    //[SerializeField] LevelManager levelManager;

    [SerializeField] Sprite[] buffIcons;

   // [SerializeField] Transform textSpawn;
    //[SerializeField] ChangeColorHPBar changeColorHPBar;

   // [SerializeField] Animator animator1;
    //[SerializeField] Animator animator2;
   // [SerializeField] Animator animator3;


    private int randomFirst;
    private int randomSecond;

    private void OnEnable()
    {
        //Debug.Log("Меню появилось");
        //randomFirst = Random.Range(1, 10);
        //randomSecond = Random.Range(1, 10);
        //while (randomFirst == randomSecond)
        //{
        //    randomSecond = Random.Range(1, 10);
        //}
        randomFirst = 1;
        randomSecond = 2;

        ResetButton(firstBuffbttn);
        ResetButton(secondBuffbttn);       

        refuseBttn.image.color = new Color(1f, 1f, 1f, 1f);
        // refusetext.color = new Color(1f, 1f, 1f, 1f);

        gameController.OnShopUpdate += ConfigureFirstButton;
        gameController.OnShopUpdate += ConfigureSecondButton;
    }

    private void ConfigureFirstButton()
    {
        AssignButtonFunction(firstBuffbttn, randomFirst);
    }

    private void ConfigureSecondButton()
    {
        AssignButtonFunction(secondBuffbttn, randomSecond);
    }

    private void OnDisable()
    {
        gameController.OnShopUpdate -= ConfigureFirstButton;
        gameController.OnShopUpdate -= ConfigureSecondButton;
    }

    private void ResetButton(Button button)
    {
        // Убираем все предыдущие подписки на события
        button.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        playerStats = PlayersManagerSingletone.Instance.LocalPlayer.Stats;
        // playerEffects = PlayerController.Instance.GetComponent<Effects>();
    }

    private void AssignButtonFunction(Button button, int randomValue)
    {
        {
            TextMeshProUGUI priceText = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Image image = button.transform.Find("Sprite").GetComponent<Image>();
            TextMeshProUGUI levelText = button.transform.Find("LevelText").GetComponent<TextMeshProUGUI>();

            // Параметры, которые зависят от randomValue
            int price = 0;
            string description = "";
            Sprite icon = null;
            int levelOfEquipment = 0;
            UnityEngine.Events.UnityAction onClickAction = null;

           

            // Настраиваем параметры для каждого randomValue
            switch (randomValue)
            {
                case 1:
                    
                    if (playerStats.iceGunLvl == 0)
                    {
                        Debug.Log("Case 1.1");
                        price = 0; //3000
                        description = "Замедляет врагов";
                        icon = buffIcons[0];
                        levelOfEquipment = 1;
                        onClickAction = () =>
                        {
                            //playerEffects.SetIceGun();
                            playerStats.iceGunLvl++;
                        };

                    }
                    else if (playerStats.iceGunLvl == 1)
                    {
                        
                        price = 0; //6000
                        description = "Распадается на сосульки";
                        icon = buffIcons[0];
                        levelOfEquipment = 2;
                        onClickAction = () =>
                        {
                            //playerEffects.SetIceGun();
                            playerStats.iceGunLvl++;
                        };
                    }
                    else if (playerStats.iceGunLvl == 2)
                    {
                        price = 0; //16000
                        description = "Сосулек теперь 5";
                        levelOfEquipment = 3;
                        icon = buffIcons[0];
                        onClickAction = () =>
                        {
                            //playerEffects.SetIceGun();

                            playerStats.iceGunLvl++;
                        };

                    }
                    else if (playerStats.iceGunLvl >= 3)
                    {
                        price = 0; //12000
                        description = "Сосульки взрываются";
                        icon = buffIcons[0];
                        levelOfEquipment = 4;
                        onClickAction = () =>
                        {
                            //playerEffects.SetIceGun();
                            if (playerStats.iceGunLvl < 4)
                                playerStats.iceGunLvl++;
                        };
                    }

                    break;

                case 2:
                    if (playerStats.shieldLvl == 0)
                    {
                        Debug.Log("Case 2");
                        price = 0; //2000
                        description = "Щит. Просто щит";
                        icon = buffIcons[1];
                        levelOfEquipment = 1;
                        onClickAction = () =>
                        {
                            //playerEffects.ChangeShield(100);
                            playerStats.shieldLvl++;
                        };
                    }
                    else if (playerStats.shieldLvl == 1)
                    {
                        price = 0; //4000
                        description = "Регенерация щита";
                        icon = buffIcons[1];
                        levelOfEquipment = 2;
                        onClickAction = () =>
                        {
                            //playerEffects.ChangeShield(100);
                            playerStats.shieldLvl++;
                        };

                    }
                    else if (playerStats.shieldLvl >= 2)
                    {
                        price = 0; //4000
                        description = "Регенерация сильнее";
                        icon = buffIcons[1];
                        levelOfEquipment = 3;
                        onClickAction = () =>
                        {
                            //playerEffects.ChangeShield(100);
                            if (playerStats.shieldLvl < 3)
                                playerStats.shieldLvl++;
                        };

                    }

                    break;

                case 3:
                    if (playerStats.fireGunLvl == 0)
                    {
                        price = 5000;//5000
                        description = "Бьет по площади";
                        icon = buffIcons[2];
                        levelOfEquipment = 1;
                        onClickAction = () =>
                        {
                            playerStats.fireGunLvl++;
                            //playerEffects.SetFireGun();

                        };
                    }
                    else if (playerStats.fireGunLvl == 1)
                    {
                        price = 10000; //10000
                        description = "Увеличенный радиус";
                        icon = buffIcons[2];
                        levelOfEquipment = 2;
                        onClickAction = () =>
                        {
                            //playerEffects.SetFireGun();
                            playerStats.fireGunLvl++;
                        };
                    }
                    else if (playerStats.fireGunLvl >= 2)
                    {
                        price = 15000; //10000
                        description = "Увеличенная атака";
                        icon = buffIcons[2];
                        levelOfEquipment = 3;
                        onClickAction = () =>
                        {
                            //playerEffects.SetFireGun();
                            if (playerStats.fireGunLvl < 3)
                                playerStats.fireGunLvl++;
                        };
                    }
                    break;

                case 4:
                    if (playerStats.lightningGunLvl == 0)
                    {
                        price = 6000;//6000
                        description = "Поражает молнией";
                        icon = buffIcons[3];
                        levelOfEquipment = 1;
                        onClickAction = () =>
                        {
                            playerStats.lightningGunLvl++;
                            //playerEffects.SetLightningGun();
                        };
                    }
                    else if (playerStats.lightningGunLvl == 1)
                    {
                        price = 9000;
                        description = "Больше целей";
                        icon = buffIcons[3];
                        levelOfEquipment = 2;
                        onClickAction = () =>
                        {
                            playerStats.lightningGunLvl++;
                            //playerEffects.SetLightningGun();
                        };

                    }
                    else if (playerStats.lightningGunLvl == 2)
                    {
                        price = 15000;
                        description = "Больше целей и урона";
                        icon = buffIcons[3];
                        levelOfEquipment = 3;
                        onClickAction = () =>
                        {
                            playerStats.lightningGunLvl++;
                            //playerEffects.SetLightningGun();
                        };

                    }
                    else if (playerStats.lightningGunLvl >= 3)
                    {
                        price = 20000;
                        description = "Больше атака и радиус";
                        icon = buffIcons[3];
                        levelOfEquipment = 4;
                        onClickAction = () =>
                        {
                            if (playerStats.lightningGunLvl < 4)
                            {
                                playerStats.lightningGunLvl++;
                            }
                            //playerEffects.SetLightningGun();
                        };

                    }
                    break;
                case 5:
                    price = 5000;
                    description = "Атака +10%";
                    icon = buffIcons[4];
                    onClickAction = () => playerStats.damage += playerStats.damage / 10; ;
                    break;
                case 6:
                    price = 5000;
                    description = "Скорость +5%";
                    icon = buffIcons[5];
                    onClickAction = () =>
                    {
                        playerStats.originMovementSpeed += playerStats.originMovementSpeed / 20;
                        playerStats.movementSpeed = playerStats.originMovementSpeed;
                    };
                    break;
                case 7:
                    price = 5000;
                    description = "Угол стрельбы +10%";
                    icon = buffIcons[6];
                    onClickAction = () =>
                    {
                        playerStats.minRotAngle += playerStats.minRotAngle / 10;
                        playerStats.maxRotAngle += playerStats.maxRotAngle / 10;
                        playerStats.maxTiltAngle += playerStats.maxTiltAngle / 10;
                    };
                    break;
                case 8:
                    price = 6000;
                    description = "Макс здоровье +10%";
                    icon = buffIcons[7];
                    onClickAction = () =>
                    {
                        playerStats.maxHealth += playerStats.maxHealth / 10;
                        //changeColorHPBar.maxHp += changeColorHPBar.maxHp / 10;
                        //UIManager.Instance.livesBar.SetNewMaxHP(GameStats.Instance.maxHealth);
                    };
                    break;
                case 9:
                    if (playerStats.laserGunLvl == 0)
                    {
                        price = 4000;
                        description = "Сверхточный лазер";
                        levelOfEquipment = 1;
                        icon = buffIcons[8];
                        onClickAction = () =>
                        {
                            playerStats.laserGunLvl++;
                            //playerEffects.SetLaserGun();
                        };
                    }

                    else if (playerStats.laserGunLvl == 1)
                    {
                        price = 8000;
                        description = "Сквозной лазер";
                        levelOfEquipment = 2;
                        icon = buffIcons[8];
                        onClickAction = () =>
                        {
                            playerStats.laserGunLvl++;
                            //playerEffects.SetLaserGun();
                        };
                    }
                    else if (playerStats.laserGunLvl >= 2)
                    {
                        price = 12000;
                        description = "Мощный лазер";
                        levelOfEquipment = 3;
                        icon = buffIcons[8];
                        onClickAction = () =>
                        {
                            if (playerStats.laserGunLvl < 3)
                            {
                                playerStats.laserGunLvl++;
                            }
                            //playerEffects.SetLaserGun();
                        };

                    }
                    break;

                default:
                    Debug.LogError("Неизвестное значение randomValue: " + randomValue);
                    return;
            }

            // Устанавливаем иконку, описание и цену для кнопки
            image.sprite = icon;
            descriptionText.text = description;
            priceText.text = $"{price}";
            levelText.text = $"Уровень {levelOfEquipment}";

            // Проверяем наличие очков и добавляем слушатели
            if (playerStats.score >= price)
            {
                button.onClick.AddListener(() =>
                {
                    onClickAction?.Invoke(); // Выполняем действие баффа
                    playerStats.score -= price; // Отнимаем очки
                    //GameEventsManager.OnBuyInMarket();
                    
                    gameController.ChangeState(GameState.LevelStart);
                   // WaitForAnimations();
                });
            }
            else
            {
                button.onClick.AddListener(() =>
                {
                    //UniversalText text = ScorePool.Instance.GetText(textSpawn.position);
                    //text.Initialize(TextType.NoMoney, price - GameStats.Instance.score, "Недостаточно очков");
                });
            }
        }
    }

    public void OnButtonClick3()
    {
       // WaitForAnimations();
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //void WaitForAnimations()
    //{
    //    //levelManager.NextLevel();
    //    animator1.SetTrigger("isBuying");
    //    animator2.SetTrigger("isBuying");
    //    animator3.SetTrigger("isBuying");
    //}
}
