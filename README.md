# José Galaxy

O projeto presente neste repositório é um trabalho semestral da disciplina PCS3539-2019S1 Tecnologia de Computação Gráfica, ministrado na USP no semestre 2020.1. A implementação baseia-se em um game inspirado no Super Mario Galaxy. As principais inspirações que tiramos deste jogo são as mecânicas de como a física é implementada no jogo, permitindo por exemplo o personagem navegar entre planetas de maneira muito criativa.

O escopo do game (José Galaxy) é de uma única fase (prova de conceito) que explora as mecânicas supracitadas. Além disso, o persoangem principal (José) foi modelado e animado do zero. Para tanto utilizamos o blender para criação do personagem e a Unity para o desenvolvimento do jogo propriamente dito. O godot foi utilizado inicialmente para desenvolvimento do jogo, mas foi substituída pela engine supracitada.

## Estrutura de pastas

```
├── Jose               # Blender files da modelagem e animação do José
├── Unity              # Implementação do jogo na Unity.
├── Godot              # Implementação do jogo no Godot(Versão MVP).
```

## Rodando o jogo

- Ter a Unity instalada na versão (2019.4.1f1). Não garantimos compatibilidade com versões anteriores
- Escolher a cena **mainStage** e rodá-la na plataforma. Pode-se ainda buildar o jogo e rodá-lo através de um executável

## Comandos

- `A`, `W`, `S`, `D` - Movimentos direcionais do personagem
- `Space` - Enquanto pressionado personagem voa
- `B` - Personagem pula (feature desencorajada em favor do voo)
- `Shift` - Enquanto pressionado personagem corre
- `Q`, `E` - Gira o personagem
- `O`, `P` - Gira a câmera
- `Mouse` - Gira a câmera
