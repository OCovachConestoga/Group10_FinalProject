const apiBase = 'https://localhost:7190/api';

document.getElementById('toggleFeedbackBtn').addEventListener('click', () => {
    const form = document.getElementById('feedbackForm').classList.remove('hidden');
    document.getElementById('toggleFeedbackBtn').classList.add('hidden');
});

document.getElementById('feedbackForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const email = document.getElementById('email').value.trim();
    const message = document.getElementById('message').value.trim();
    const submittedAt = new Date().toISOString(); // Valid format for C#

    const feedback = {
        userEmail: email,
        message: message,
        submittedAt: submittedAt
    };

    try {
        const response = await fetch(`${apiBase}/feedback/submit`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(feedback) // No need to wrap in { feedback: ... }
        });

        if (response.ok) {
            alert('Feedback submitted successfully!');
            e.target.reset();
            e.target.classList.add('hidden'); // hide after submission
            document.getElementById('toggleFeedbackBtn').classList.remove('hidden');
        } else {
            alert('Failed to submit feedback.');
        }
    } catch (err) {
        console.error('Error:', err);
        alert('An error occurred.');
    }
});


document.getElementById('loadAnalytics').addEventListener('click', async () => {
    try {
        // Fetch user count analytics data
        const usersResponse = await fetch(`${apiBase}/analytics/users`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (usersResponse.ok) {
            const usersData = await usersResponse.json();
            
            // Process and display the users data
            let userStatsOutput = `
                <div style="display: flex; align-items: center; gap: 20px;">
                    <h3>Current User Count:</h3>
                    <p>${usersData}</p>
                </div>
            `;

            // Update the DOM with the formatted output for users
            document.getElementById('userStatsOutput').innerHTML = userStatsOutput;
        } else {
            document.getElementById('userStatsOutput').textContent = 'Failed to load user statistics.';
        }

        const response = await fetch(`${apiBase}/analytics/games`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const data = await response.json();
            
            // Check if the data is valid
            if (Array.isArray(data) && data.length > 0) {
                let output = '<h3>Created Games Stats (By Month)</h3><table class="striped-table"><thead><tr><th>Month</th><th>Games Created</th></tr></thead><tbody>';

                // Process and display the data
                data.forEach(item => {
                    // Convert the "month" value (which is in "YYYY-MM" format) to month name + year
                    const monthYear = new Date(item.month + '-01'); // Add '-01' to make it a valid date
                    const monthName = monthYear.toLocaleString('default', { month: 'long' }); // Get full month name
                    const year = monthYear.getFullYear(); // Get the year

                    // Format as "Month Year" (e.g., "January 2025")
                    const formattedMonthYear = `${monthName} ${year}`;

                    output += `<tr><td>${formattedMonthYear}</td><td>${item.gameCount}</td></tr>`;
                });

                output += '</tbody></table>';

                // Update the DOM with the formatted output
                document.getElementById('analyticsOutput').innerHTML = output;
            } else {
                document.getElementById('analyticsOutput').textContent = 'No data available for the requested period.';
            }
        } else {
            document.getElementById('analyticsOutput').textContent = 'Failed to load analytics.';
        }

        const locationStatsResponse = await fetch(`${apiBase}/analytics/locations`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (locationStatsResponse.ok) {
            const locationData = await locationStatsResponse.json();

            if (Array.isArray(locationData) && locationData.length > 0) {
                // Group by month
                const groupedByMonth = {};
                locationData.forEach(item => {
                    if (!groupedByMonth[item.month]) {
                        groupedByMonth[item.month] = [];
                    }
                    groupedByMonth[item.month].push(item);
                });

                let output = '<h3>Top 3 Locations (By Month)</h3><table class="striped-table"><thead><tr><th>Month</th><th>Top 3 Locations</th></tr></thead><tbody>';

                Object.entries(groupedByMonth).forEach(([monthKey, locations]) => {
                    const [month, year] = monthKey.split('-');
                    const date = new Date(`${year}-${month.padStart(2, '0')}-01`);
                    const monthName = date.toLocaleString('default', { month: 'long' });
                    const formattedMonth = `${monthName} ${year}`;

                    locations
                        .sort((a, b) => b.count - a.count) // Sort by count
                        .slice(0, 3) // Take top 3
                        .forEach((location, index) => {
                            output += '<tr>';
                            if (index === 0) {
                                output += `<td rowspan="${Math.min(3, locations.length)}">${formattedMonth}</td>`;
                            }
                            output += `<td>${location.location} - ${location.count} games</td></tr>`;
                        });
                });

                output += '</tbody></table>';
                document.getElementById('locationStatsOutput').innerHTML = output;
            } else {
                document.getElementById('locationStatsOutput').textContent = 'No data available.';
            }
        } else {
            document.getElementById('locationStatsOutput').textContent = 'Failed to load location trends.';
        }
    } catch (err) {
        console.error('Error:', err);
        document.getElementById('locationStatsOutput').textContent = 'An error occurred while fetching analytics.';
    }
});

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('loadAnalytics').click();
});

function cancelForm(){
    document.getElementById('feedbackForm').classList.toggle('hidden');
    document.getElementById('toggleFeedbackBtn').classList.remove('hidden');
    document.getElementById('cancelbtn').classList.add('hidden');
}