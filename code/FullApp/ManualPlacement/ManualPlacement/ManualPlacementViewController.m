//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"
@interface ManualPlacementViewController()
    @property (nonatomic,strong) UIActionSheet *actionSheet;

@end



@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize tapRecognizer = _tapRecognizer;
@synthesize imageView = _imageView;
@synthesize actionSheet = _actionSheet;


int currentOp = 1;

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
    [self.view addGestureRecognizer:[self tapRecognizer]];
    NSLog(@"Logged");
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}
- (IBAction)tapped:(id)sender {
    
}



     -(void) touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
    {
        //Finger Down
        UITouch *anyTouch = [touches anyObject];
        if (anyTouch.tapCount == 1) 
        {
            //Create a new Smile
            NSLog(@"tapped at %f, %f",[self.tapRecognizer locationInView:self.view].x,[self.tapRecognizer locationInView:self.view].y);  
            
        }else if (anyTouch.tapCount == 2) 
        {
            //Create a new Smile
            NSLog(@"twice");        
        }else if (anyTouch.tapCount == 3) 
        {
            //Create a new Smile
            NSLog(@"thrice");        
        }
    }

- (UITapGestureRecognizer *) tapRecognizer{
    if (!_tapRecognizer) _tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapped:)];
    return _tapRecognizer;
}


- (UIActionSheet *)actionSheet
{
    if (!_actionSheet) _actionSheet = [[UIActionSheet alloc] initWithTitle:@"Select option" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:@"Remove point" otherButtonTitles:@"Add point",@"Move point", nil];
    return _actionSheet;
}

- (IBAction)actionButtonPress:(id)sender {
    [self.actionSheet showInView:self.view];                                  
                                  
}

- (void)actionSheet: (UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
    //NSLog(@"CurrentOp was %d",currentOp);
    //NSLog(@"CurrentOp = %d",currentOp);
    if (buttonIndex == 3) //Remove points
    {
        NSLog(@"Cancelled op");
    }
    else 
    {
        currentOp = buttonIndex;
    }
}


- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)viewDidUnload {
    [self setImageView:nil];
    [self setTapRecognizer:nil];
    [super viewDidUnload];
}
@end
