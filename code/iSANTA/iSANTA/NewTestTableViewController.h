//
//  NewTestTableViewController.h
//  iSANTA
//
//  Created by Jack Hall on 10/22/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "MasterViewController.h"

@interface NewTestTableViewController :  UITableViewController <UINavigationControllerDelegate,UIImagePickerControllerDelegate, UIAlertViewDelegate, UIPickerViewDelegate, UIPickerViewDataSource>

@property (retain, nonatomic) UIImage *targetImage;
@property (retain, nonatomic) NSIndexPath *selectedIndexPath;
@property (retain, nonatomic) NSMutableArray *arrayTemps;
@property (retain, nonatomic) UIPickerView *tempPicker;

-(IBAction)dismissNewTestModalView:(id)sender;


@end
