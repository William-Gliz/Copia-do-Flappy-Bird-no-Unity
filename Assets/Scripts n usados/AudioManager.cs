using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Pareceido com o Start, mas é chamado antes
    void Awake()
    {
        // Verifica se o objeto AudioManager existe na cena, e evita que ele seja criado mais
        // de uma vez, quando a cena for carregada
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        // Para evitar que a musica seja cortada entre cenas:
        DontDestroyOnLoad(gameObject);

        // Pra cada som, criaremos um audio souce
        foreach (Sound s in sounds)
        {
            //Vamos adicionar um componente no objeto atual (gameobject)
            // O objeto atual é o próprio Audio Manager
            s.source = gameObject.AddComponent<AudioSource>();

            // As características do componente serão as características que nós adicionamos
            // Clip = O clip que será adicionado
            // Volume = Volume selecionado no slider
            // Pitch = Pitch selecionado no slider
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("Musica");   
    }

    // Método para tocar o som, que será chamado de fora da classe
    public void Play (string name)
    {
        // Dentro desse método, vamos procurar o nome do clip que será tocado
        // Poderia ser feito novamente com "foreach", 
        // mas esse é outro jeito de fazer usando System


        // Dentro do Array sounds, vamos encontrar o som cujo nome é o nome que nós queremos
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)  // Caso não exista um som com o nome que foi passado
        {
            // Para avisar que o soum não existe:
            Debug.LogWarning("Sound " + name + " não existe!");
            return;
        }
        s.source.Play();

    }

    
}
