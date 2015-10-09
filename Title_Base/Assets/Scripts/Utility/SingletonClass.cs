/****************************************************************************/
/*!
\file   SingletonClass.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the input manager class
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

                                           // restrict to monobehaviour 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
  // we should never make a base singleton class
  protected Singleton () {}

  // we need this to tell us when the game has begun to close
  private static bool GameClosing = false;

  // the instance of this singleton,
  private static T SingletonInstance = null;

  // we'll set up a lock just in case unity multithreads stuff 
  private static object MyLock = new object();

  /****************************************************************************/
  /*!
  \brief  
      Returns the singleton instance.   If the singleton hasn't been created
      yet, it will be created, if there is already a singleton, that will be 
      returned.
  */
  /****************************************************************************/
  public static T GetSingleton
  {
    get
    {
      // if the game is closing return null
      if(GameClosing)
      {
        print("this would be bad");

        return null;
      }

      // this is where multithreading could cause an issue, lock the critical section just in case
      lock (MyLock)
      {
        if (SingletonInstance == null)
        {
          // sanity check to ensure we didn't create multiple singletons
          SingletonInstance = (T)FindObjectOfType(typeof(T));

          if(FindObjectsOfType(typeof(T)).Length > 1)
          {
            print("Something really bad happened we have multiple singletons");

            return SingletonInstance;
          }

          // first time call initialize the class.
          else if (SingletonInstance == null)
          {
            // create a blank gameobject 
            GameObject newsingleton = new GameObject();

            // add the component to the new gameobject
            SingletonInstance = newsingleton.AddComponent<T>();

            // rename the object so each singleton has an exclusive name
            newsingleton.name = "(singleton) " + typeof(T).ToString();

            // we don't want to lose the singleton on load only on close
            DontDestroyOnLoad(newsingleton);
          }
        }
      }

      // default case, return the singleton
      return SingletonInstance;
    }
  }

  /****************************************************************************/
  /*!
  \brief  
      Called when the game ends, ensures that we won't get unintended behavior
      when the game closes
  */
  /****************************************************************************/
  public void OnDestroy()
  {
    // this way, we will know when the game is being closed, and can prevent other scrips from trying to access this.
    GameClosing = true;
  }
}
