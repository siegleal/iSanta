//
//  NewTestViewController.h
//  iSanta
//
//  Created by Jack Hall on 1/10/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface NewTestViewController : UIViewController <UISplitViewControllerDelegate, UITableViewDelegate, UITableViewDataSource, UIImagePickerControllerDelegate, UIAlertViewDelegate, UINavigationControllerDelegate, UIActionSheetDelegate>

@property (nonatomic, retain) IBOutlet UITableView *reportTable;

@property (nonatomic, retain) IBOutlet UIBarButtonItem *cancelBB;

@property (nonatomic, retain) IBOutlet UIBarButtonItem *doneBB;

@property (nonatomic, retain) NSFetchedResultsController *fetchedResultsController;

@property (nonatomic, retain) NSManagedObjectContext *context;

@property (nonatomic, retain) NSManagedObject *testReportObject;
@property (nonatomic, retain) NSManagedObject *shooterObject;
@property (nonatomic, retain) NSManagedObject *rangeObject;
@property (nonatomic, retain) NSManagedObject *weaponObject;
@property (nonatomic, retain) NSManagedObject *ammoObject;
@property (nonatomic, retain) NSManagedObject *photoObject;

@property (nonatomic, retain) NSIndexPath *selectedIndexPath;

@property (retain, nonatomic) UIImage *targetImage;

-(IBAction)dismissNewTestModalView:(id)sender;

-(IBAction)cancelNewTest:(id)sender;

-(IBAction)saveNewTest:(id)sender;

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath;


@end
