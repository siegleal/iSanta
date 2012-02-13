//
//  DetailViewController.h
//  iSanta
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "MasterViewController.h"
#import "DetailTableViewCell.h"

@interface DetailViewController : UIViewController <UISplitViewControllerDelegate, UITableViewDelegate, UITableViewDataSource, UIImagePickerControllerDelegate, UIAlertViewDelegate, UINavigationControllerDelegate, UIActionSheetDelegate, UITextFieldDelegate, UIPickerViewDataSource, UIPickerViewDelegate>

@property (strong, nonatomic) id detailItem;
@property (nonatomic) UIImagePickerControllerSourceType imagePickerSourceType;
@property (nonatomic) UIImagePickerControllerCameraCaptureMode imagePickerCaptureMode;
@property (nonatomic) UIImagePickerControllerCameraDevice imagePickerCameraDevice;

@property (nonatomic, retain) IBOutlet UITableView *detailDescriptionTable;

@property (retain, nonatomic) UIImage *targetImage;
@property (nonatomic, retain) NSIndexPath *selectedIndexPath;

@property (nonatomic, retain) UIToolbar *doneToolBar;

@property (nonatomic, retain) UIPickerView *temperaturePicker;

@property (nonatomic, retain) UITextField *respondingTextField;

- (IBAction)dismissKeyboard:(id)sender;

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath;

@end
