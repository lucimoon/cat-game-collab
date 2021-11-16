using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IrritationBar : MonoBehaviour
{
  public Image irritationBarImage;
  public NonPlayerController nonPlayerController;

  public void UpdateIrritationBar()
  {
    float duration = 0.75f * (nonPlayerController.irritationScore / nonPlayerController.maxIrritation);
    irritationBarImage.DOFillAmount(nonPlayerController.irritationScore / nonPlayerController.maxIrritation, duration);

    Color red = new Color(0.6981f, 0.5160248f, 0.4625756f, 1f);
    Color green = new Color(0.494838f, 0.754717f, 0.6536991f, 1f);
    Color gray = new Color(0.6981f, 0.6981f, 0.6981f, 1f);

    Color barColor = gray;

    if (nonPlayerController.irritationScore < nonPlayerController.startingIrritation)
    {
      barColor = green;
    }
    else if (nonPlayerController.irritationScore > nonPlayerController.startingIrritation)
    {
      barColor = red;
    }

    irritationBarImage.DOColor(barColor, duration);
  }
}
