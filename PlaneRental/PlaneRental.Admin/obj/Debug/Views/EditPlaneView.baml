   M S B A M L   `   `   `  ����  PlaneRental.Admin+    %PlaneRental.Admin.Views.EditPlaneView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      I ECore.Common.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null*corehttp://www.PluralSight.com/Core  5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��40 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   .  (Core.Common.UI.Controls.ValidationHeader  $��0��6     � DataContext 6-   �� � 5   
   $��1��6     5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $��80 �6#   �� 5      $	��180 �6#   .+�5      �� $��0��6   $��0��6%   $��3q�65   $��Description:��6@   �� 5      $��1��6   $��0��6%   $��3q�65   $��Color:��6@   �� 5      $��2��6   $��0��6%   $��3q�65   $��Year:��6@   �� 5      $��3��6   $��0��6%   $��3q�65   $��Per-Day Price:��6@   �� 5      $��0��6   $��1��6#   $��3q�63   $	��160��6>   $
��Left=�6�   ��6J   �� *Plane.Description+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . 6�   �� 5       $��1��6   $��1��6#   $��3q�63   $��70��6>   $
��Left=�6�   ��6I   �� *Plane.Color+$ PropertyChanged=� . 6�   �� 5!      $��2��6   $��1��6#   $��3q�63   $��60��6>   $
��Left=�6�   ��6I   �� *
Plane.Year+$ PropertyChanged=� . 6�   �� 5"      $��3��6   $��1��6#   $��3q�63   $��80��6>   $
��Left=�6�   ��6I   �� *Plane.RentalPrice+$ PropertyChanged=� . 6�   5#      �� 5$   
   $��2��6    �� Orientation$ 
Horizontal=�6"   .+�5%      �� $
��Save��6   $��5q�6$   ��6/   �� *SaveCommand+�� 5&      $��Cancel��6   $��5q�6&   ��61   �� *CancelCommand+5'      5(      5)      