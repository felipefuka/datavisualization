   M S B A M L   `   `   `  ����G  CSharpGL.WPF, Version=1.99.1.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPFSharpGL.WPF  5   	   ^ ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers 5       MetricSPlat_Kinect#   MetricSPlat_Kinect.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   BsharpGL.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPF     5   	   mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     5      -   5          � Title$)  #Visualização de Dados v0.1 - ICMC��5   	   $	��570��63   $	��868��6@    � WindowState$ Normal=�5   "    � 
ResizeMode$ NoResize=�67   .��5	      �� -   5        $��
MainCanvas��5	      $	��491��6   $	��846��6,   .+�5   
   o� -   5        $��	testemenu��5      $��22��6    $	��867��6,    �� Left$ -9��68   
 �� Top$	 -24��6I   .@�5   
   m� $
��File��6   .@�5      m� -   5        $
��Open��5      5      m� 5      -   5        $
��Exit��5      5      5      m� 5   
   $
��Help��6   .@�5      m� -   5        $��	View Help��5      5      m� 5      -   5        $��About��5      5      5      5      ;  5Microsoft.Samples.Kinect.WpfViewers.KinectColorViewer  5   
   -   5        $��kinectColorViewer1��5   X   $
 -199��6   $	 572��62   $��0��6C   $��0��6M    K� ZIndex$ 5��5   `   5  /Microsoft.Samples.Kinect.WpfViewers.ImageViewer   Kinect 6   ��  �� ElementName$ kinectSensorChooser1�� �� Path$ Kinect�6`   =  7Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser  5   
   -	   5        $��kinectSensorChooser1��5   E   $
 -164��6!   $	 608��64   $��0��6a   $��0��6k   $ 5��6v      SharpGL.WPF.OpenGLControl  5   
   -
   5        $��openGLProjectionControl��5   I   $ 12��6    $	��600��60   $	��474��6<   $	 -10��5   E   $	 234��6X   6�   �� 5!   
   -   5        $��button1��5!   T   $ 93��6   $	 125��6"   $��
Rotate Z +��63   $��23��6H   $��75��6c   6n   �� 5"   
   -   5        $��button2��5"   T   $ 12��6   $	 125��6"   $��
Rotate Z -��63   $��23��6H   $��75��6c   6n   �� 5#   
   -   5        $��button3��5#   T   $ 12��6   $	 154��6"   $��
Rotate Y -��63   $��23��6H   $��75��6c   6n   �� 5$   
   -   5        $��button4��5$   T   $ 93��6   $	 154��6"   $��
Rotate Y +��63   $��23��6H   $��75��6c   6n   �� 5%   
   -   5        $��button5��5%   P   $ 93��6   $	 183��6"   $��Zoom +��63   $��23��6D   $��75��6_   6�   �� 5&   
   -   5        $��button6��5&   P   $ 12��6   $	 183��6"   $��Zoom -��63   $��23��6D   $��75��6_   6j   �� 5'   
   -   5        $��	sliderRes��5'   ?   $ 12��6   $	 425��6"   $��25��63   $	��201��6P   	 � Maximum$	 30Q�6\   �� 5(   
   -   5        $��button7��5(   P   $ 12��6   $	 456��6"   $��Button��63   $��23��6D   $��75��6_   6j   5)      5*      