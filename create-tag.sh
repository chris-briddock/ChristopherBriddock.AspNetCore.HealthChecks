#!/bin/bash

# Function to print colored output
print_message() {
    GREEN='\033[0;32m'
    NC='\033[0m' # No Color
    echo -e "${GREEN}$1${NC}"
}

# Function to print error message and exit
print_error() {
    RED='\033[0;31m'
    NC='\033[0m' # No Color
    echo -e "${RED}Error: $1${NC}"
    exit 1
}

# Check if version argument is provided
if [ $# -eq 0 ]; then
    print_error "Please provide a version number: ./create-tag.sh v1.0.0"
fi

VERSION=$1

# Validate version format (vX.Y.Z)
if ! [[ $VERSION =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    print_error "Version must be in format vX.Y.Z (e.g., v1.0.0)"
fi

# Ensure we're in a git repository
if ! git rev-parse --git-dir > /dev/null 2>&1; then
    print_error "Not a git repository"
fi

# Switch to main branch
print_message "Switching to main branch..."
git checkout main || print_error "Failed to switch to main branch"

# Pull latest changes
print_message "Pulling latest changes..."
git pull origin main || print_error "Failed to pull latest changes"

# Check if tag already exists
if git rev-parse "$VERSION" >/dev/null 2>&1; then
    print_error "Tag $VERSION already exists"
fi

# Create and push tag
print_message "Creating tag $VERSION..."
git tag -a "$VERSION" -m "Release $VERSION" || print_error "Failed to create tag"

print_message "Pushing tag to remote..."
git push origin "$VERSION" || print_error "Failed to push tag"

print_message "\nTag $VERSION has been created and pushed successfully!"
print_message "You can view it at: $(git remote get-url origin | sed 's/\.git$//')/releases/tag/$VERSION"