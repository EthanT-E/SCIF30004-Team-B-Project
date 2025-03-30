# SCIF30004-Team-B-Project
## Description
An interactive VR teaching tool made for A-level or equivalent physics students to visualise:
-	Magnetic fields.
-	The changes to existing magnetic fields when introducing more magnets.
-	The effect of magnet properties such as magnetic susceptibility and auxiliary field on field shape and strength.
-	Interactive forces between magnets.
-	Haptics to feel the forces exerted on pairs of magnets
## Installation and start up Guide
### Prerequisites
- USB-C cable
- SideQuest installed on a PC (useful guide: https://www.youtube.com/watch?v=lFTXv2aScJ8)
- Quest 2 with developer mode enabled (useful guide: https://www.youtube.com/watch?v=TWHrvQ3VTJQ)
### Installation steps
1)	Go to https://github.com/EthanT-E/SCIF30004-Team-B-Project/releases/tag/1.0.0 and download MagnetVisDemo.apk.
![download](https://github.com/user-attachments/assets/a7b37a1a-fe02-453c-b833-1fd8e2d3e393)
2) Connect the Quest 2 and PC with the USB – C cable and then on the headset press allow on the pop-up.
3) On SideQuest click on the ‘install APK from folder on computer’ button then select the downloaded MagnetVisDemo.apk and wait until SideQuest says the task is complete.
![side view](https://github.com/user-attachments/assets/dcde745f-6922-40d3-bf58-924e602ff980)
4)  On the headset click on Library (far left on the bar) and then navigate to ‘Unknown sources’ and select B-Field Prototype.
![unkown](https://github.com/user-attachments/assets/95aa866e-208d-4e6f-971e-e06f5f7b11a7)
## User Guide 
### Controls
![Controls](https://github.com/user-attachments/assets/18823c1f-8348-4447-9663-3bd46eb5a287)
-	Trigger-Interact/Grab magnet
-	Primary button-Select magnet to edit
-	Secondary button-Toggle scanner
-	
 ![Menu](https://github.com/user-attachments/assets/d2b198e1-bfbf-4355-b857-db2abe77e4e0)
### Generate Magnets
1)	On the whiteboard select the Generate option.
2)	Drag the sliders across to the desired values.
3)	Select generate.
### Edit/delete magnet
1)	Point at the magnet(s) and press the primary button (A/X). The magnet(s) should become highlighted. 
2)	From the menu select Edit.
3)	Move the sliders to change the parameters and then deselect the magnet using the same controls as selecting it, or to delete the magnet: press ‘Delete’ in the edit menu.
![before selectd](https://github.com/user-attachments/assets/a18a97f6-7205-4a33-92c4-573f411b99f4)
![after selected](https://github.com/user-attachments/assets/639ad904-2eba-4ae0-9032-63c0bff51aa1)
### Tutorial 
-	The program automatically starts the whiteboard with the Tutorial. 
-	To replay the tutorial select it in the whiteboard menu.
### Forces 
-	Interactive forces between magnets are shown on non-held magnets.
-	If two magnets are held the strength of the force can be felt through the haptics of the controllers. 
-	Note if the magnetic susceptibility and auxiliary field are low or the magnets are far apart, the forces will be very weak.
-	**Calculated with the formula** $${F} = \frac{3\mu_0|{m_a}|{|m_b}|}{4\pi}(\frac{\hat{r}(\hat{m_a}\cdot\hat{m_b})+\hat{m_a}(\hat{r}\cdot\hat{m_b})+{\hat{m_b}}(\hat{r}\cdot\hat{m_a})-5\hat{r}(\hat{r}\cdot\hat{m_a})(\hat{r}\cdot\hat{m_b})}{|{r}|^4}$$ (1) 
### Scanner 
![scanner](https://github.com/user-attachments/assets/c8ee27c8-94b1-4d1c-91e9-b9f4caa1c138)
1)	Press the secondary button (Y/B) the controller should switch to a scanner.
2)	Move the point at the end of the scanner’s antenna to the location that you want to measure the magnetic field at and press the trigger.
3)	To exit scanner mode, press the secondary button again.
-	**Calculated magnetic field from a dipole with** $${B} =(\frac{\mu_0}{4\pi})\cdot(\frac{3[(m\cdot\hat{r})\hat{r}-m]}{|r|^3})$$ (2)
## Credits
### Supervisor
Dr Simon Hanna
### Made by
Adam Harrison, Qingyang Liu, Alex Nereča, Ethan Tripp-Edwards and Nathan Truong
## References
1) Yung K.W., Landecker, P.B. & Vilani, D.D *An analytic solution for the force between two magnetic dipoles.* Physical Separation in Science and Engineering. 9(I). 39-52
2) Griffiths, D. J. (2017). Introduction to Electrodynamics (4th ed.). Cambridge: Cambridge University Press. 
