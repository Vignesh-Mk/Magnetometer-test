# 🧲 Magnet Polarity & Intensity Detection

A high-precision mobile solution built in **Unity 6000.3.8f1** for detecting, classifying, and measuring magnetic fields using onboard device hardware.

---

## 📱 Supported Platforms
* **Android:** native magnetometer integration.
* **iOS:** custom hardware bridge via CoreMotion.
* *Note: Target devices must be equipped with a functional magnetometer sensor.*

---

## 🚀 Key Features
* **Polarity Classification:** Accurately identifies North vs. South magnetic poles.
* **Intensity Mapping:** Outputs real-time magnetic flux density as a precise `float` value.
* **Cross-Platform Architecture:** Uses a modular `AppManager` system to handle OS-specific sensor data.

---

## 🛠️ Technical Implementation

### **iOS Native Bridge**
To overcome standard limitations and access the **CoreMotion Library** directly, this project includes:
* **`Magnetometer.mm`**: A custom C-based hardware interface written from scratch to pull raw sensor data from the iOS kernel.
* **C# Wrapper**: A dedicated bridge that allows `MonoBehaviour` scripts to interpret native C values within the Unity environment.

### **Android Integration**
Uses a specialized `AppManager` optimized for the Android Sensor API to stream XYZ magnetic coordinates into the engine.

---

## 💻 Languages & Tools
| Component | Technology |
| :--- | :--- |
| **Engine** | Unity 6000.3.8f1 |
| **Primary Logic** | C# |
| **iOS Plugin** | C / Objective-C |
| **API** | CoreMotion (iOS) / Android Sensor API |

---

## 📦 Installation & Setup
1. **Clone the Repository:**
   ```bash
   git clone [https://github.com/your-username/magnet-detection.git](https://github.com/your-username/magnet-detection.git)
