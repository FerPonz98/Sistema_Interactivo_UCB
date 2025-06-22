# Sistema_Interactivo_UCB

## Descripción

Este proyecto consiste en el desarrollo de un **simulador educativo en realidad virtual (VR)** diseñado para la enseñanza de conceptos relacionados con **automatización industrial**, enfocándose en sistemas **neumáticos** controlados por **PLC**. El simulador busca mejorar la comprensión de los estudiantes mediante un entorno inmersivo que permite la interacción directa con componentes virtuales como **cilindros neumáticos** y **válvulas**.

El proyecto está orientado a estudiantes de **Ingeniería Mecatrónica** y otras áreas afines, facilitando el aprendizaje de **simbología**, **conexiones** y **programación** mediante retos de distinta dificultad.

## Tecnologías utilizadas

- **Unity** (motor de desarrollo principal)
- **XR Interaction Toolkit** (para interacción VR)
- **Meta Quest 2 y Meta Quest 3** (dispositivos objetivo)
- **Blender / Autodesk Fusion 360** (modelado de componentes)
- **C#** (lenguaje de programación)
- **OpenXR** (estándar multiplataforma para VR)

## Instalación

### Requisitos previos

- **Unity 2021.3 LTS** o superior con soporte para XR
- **Dispositivo Meta Quest 2 o 3**
- **Cable o conexión inalámbrica** para realizar pruebas (Air Link o similar)

### Pasos básicos


# Clonar el repositorio (cuando esté publicado en GitHub o similar)
git clone https://github.com/FerPonz98/Sistema_Interactivo_UCB.git


cd Sistema_Interactivo_UCB


1️⃣ Abrir el proyecto en **Unity**


2️⃣ Configurar el **Player Settings** para Meta Quest (OpenXR activado)


3️⃣ Hacer **build** para Android y probar en el visor

## Uso

Una vez desplegada la aplicación:

* Selecciona el modo de juego: **Libre** o **Contrarreloj**
* Resuelve los retos conectando los componentes neumáticos según las preguntas planteadas
* El sistema registra el tiempo y la solución elegida
* Los niveles avanzan en dificultad con diferentes tipos de cilindros, válvulas y circuitos

## Estructura del simulador

* **Panel de inicio**: Interfaz para elegir modo de juego
* **Panel de juego**: Escenario inmersivo con prefabs interactivos
* **Panel final**: Resumen del tiempo empleado y retroalimentación
* **Modo difícil**: Incluye tres soluciones posibles por pregunta, con retroalimentación visual

## Autores

* **Fernando Ponz** (Desarrollador principal, Ingeniería Mecatrónica)
* **Universidad Católica Boliviana** (Proyecto educativo)

## Licencia

Este proyecto es de uso académico y no cuenta con licencia de distribución comercial en esta versión.

```

